using GoPlayServer.Data;
using GoPlayServer.DTOs;
using GoPlayServer.Entities;
using GoPlayServer.Hubs;
using GoPlayServer.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace GoPlayServer.Controllers
{
    [Route("chat")]
    [ApiController]
    [Authorize]
    public class ChatController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IHubContext<MessageHub> hubContext;
        private readonly IUserRepository _userRepo;
        private readonly IPostRepository _postRepo;
        private readonly IMessageRepository _messageRepo;
        private readonly IGroupRepository _groupRepo;

        public ChatController(IHubContext<MessageHub> hubContext, IMessageRepository messageRepo, AppDbContext context, IGroupRepository groupRepo, IUserRepository userRepo, IPostRepository postRepo)
        {
            this.hubContext = hubContext;
            _messageRepo = messageRepo;
            _context = context;
            _groupRepo = groupRepo;
            _userRepo = userRepo;
            _postRepo = postRepo;
        }

        [HttpPost("messageGroup")]
        public async Task MessageGroup(MessageDTO messageDto)
        {
            await this.hubContext.Clients.Group(messageDto.groupName).SendAsync("RecieveMessage", messageDto);

            var group = _context.Groups.SingleOrDefault(g => g.groupName == messageDto.groupName);
            if (group == null) return;
            if (group.messages == null) group.messages = new List<Message>();

            var message = new Message {
                text = messageDto.text,
                userName = messageDto.username,
                date = messageDto.dateTime.AddHours(2),
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

        [HttpGet("getGroupsFor")]
        public async Task<ActionResult<List<string>>> GetGroupsFor(string username)
        {
            IQueryable<Group> groups = await _groupRepo.GetGroupsForUser(username);

            if (groups is null) return new BadRequestResult();

            List<string> groupNames = new List<string>();

            foreach (var group in groups)
            {
                groupNames.Add(group.groupName);
            }

            return Ok(groupNames);
        }

        [HttpPost("removeFromGroup")]
        public async Task<ActionResult> RemoveFromGroup(string username, string groupname)
        {
            await _groupRepo.RemoveFromGroup(username, groupname);
            return Ok();
        }
    }
}
