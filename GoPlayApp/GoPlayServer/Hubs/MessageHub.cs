using GoPlayServer.DTOs;
using GoPlayServer.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace GoPlayServer.Hubs
{
    public class MessageHub : Hub
    {
        private readonly IMessageRepository _messageRepo;

        public MessageHub(IMessageRepository messageRepo)
        {
            _messageRepo = messageRepo;
        }

        public async Task<ICollection<MessageDTO>> AddToGroup(string groupname)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, groupname);
            return _messageRepo.GetMessagesIn(groupname);
        }

        /*public Task SendMessageToGroup(string groupname, MessageDTO message)
        {
            return Clients.Group(groupname).SendAsync("RecieveMessage", message);
        }*/
    }
}
