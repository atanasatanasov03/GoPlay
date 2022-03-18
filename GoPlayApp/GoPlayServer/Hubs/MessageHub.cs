using GoPlayServer.Data;
using GoPlayServer.DTOs;
using GoPlayServer.Entities;
using GoPlayServer.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace GoPlayServer.Hubs
{
    public class MessageHub : Hub
    {

        private readonly AppDbContext _context;
        private readonly IMessageRepository _messageRepo;
        private readonly IGroupRepository _groupRepo;
        private readonly IUserRepository _userRepo;

        public MessageHub(AppDbContext context, IMessageRepository messageRepo, IGroupRepository groupRepo, IUserRepository userRepo)
        {
            _context = context;
            _messageRepo = messageRepo;
            _groupRepo = groupRepo;
            _userRepo = userRepo;
        }

        public async Task<ICollection<MessageDTO>> AddToGroup(string groupname, string username)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, groupname);
            
            var group = await _groupRepo.GetGroupByName(groupname);
            var user = await _userRepo.GetUserByUsernameAsync(username);

            IQueryable<AppUser> usersInGroup = await _groupRepo.GetUsersInGroup(groupname);

            if (!usersInGroup.Contains(user))
            {
                group.users.Add(user);
                await _context.SaveChangesAsync();
            }

            return _messageRepo.GetMessagesIn(groupname);
        }
    }
}
