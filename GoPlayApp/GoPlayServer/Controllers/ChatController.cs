using GoPlayServer.Data;
using GoPlayServer.DTOs;
using GoPlayServer.Entities;
using GoPlayServer.Hubs;
using GoPlayServer.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace GoPlayServer.Controllers
{
    [Route("chat")]
    [ApiController]
    public class ChatController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IHubContext<MessageHub> hubContext;
        private readonly IMessageRepository _messageRepo;

        public ChatController(IHubContext<MessageHub> hubContext, IMessageRepository messageRepo, AppDbContext context)
        {
            this.hubContext = hubContext;
            _messageRepo = messageRepo;
            _context = context;
        }

        /*[HttpPost("/message")]
        public async Task SendMessage(MessageDTO message)
        {
            await this.hubContext.Clients.All.SendAsync("messageReceivedFromApi", message);
        }*/


        [HttpPost("messageGroup")]
        public async Task MessageGroup(MessageDTO messageDto)
        {
            await this.hubContext.Clients.Group(messageDto.groupName).SendAsync("RecieveMessage", messageDto);

            var group = _context.Groups.SingleOrDefault(g => g.groupName == messageDto.groupName);
            if (group == null) return;
            if (group.messages == null) group.messages = new List<Message>();

            var message = new Message {
                text = messageDto.Text,
                userName = messageDto.username,
                date = messageDto.DateTime,
                group = group,
                GroupId = group.Id
            };
            group.messages.Add(message);

            _messageRepo.SaveMessage(message);
            _context.Groups.Update(group);
            await _context.SaveChangesAsync();
        }

        [HttpGet("getMessagesFor")]
        public ActionResult<List<MessageDTO>> GetMessagesFor(string groupname)
        {
            ICollection<MessageDTO> messages = _messageRepo.GetMessagesIn(groupname);

            //if (messages == null) return new EmptyResult;
            //else
            return Ok(messages);
        }
    }
}
