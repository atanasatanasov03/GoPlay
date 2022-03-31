using GoPlayServer.Data;
using GoPlayServer.DTOs;
using GoPlayServer.Entities;
using GoPlayServer.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Net.Mail;

namespace GoPlayServer.Hubs
{
    public class MessageHub : Hub
    {

        private readonly AppDbContext _context;
        private readonly IMessageRepository _messageRepo;
        private readonly IGroupRepository _groupRepo;
        private readonly IUserRepository _userRepo;
        private readonly IConfiguration _config;

        public MessageHub(AppDbContext context, IMessageRepository messageRepo, IGroupRepository groupRepo, IUserRepository userRepo, IConfiguration config)
        {
            _context = context;
            _messageRepo = messageRepo;
            _groupRepo = groupRepo;
            _userRepo = userRepo;
            _config = config;
        }

		public async Task<ICollection<MessageDTO>> AddToGroup(string groupname, string username)
		{
			await Groups.AddToGroupAsync(Context.ConnectionId, groupname);

			var group = await _groupRepo.GetGroupByNameAsync(groupname);
			var post = await _context.Posts.SingleOrDefaultAsync(p => p.groupId == group.Id);
			var user = await _userRepo.GetUserByUsernameAsync(username);

			var userInGroup = await _groupRepo.UserInGroup(username, groupname);

			if (userInGroup.Equals("false"))
			{
				var admin = await _userRepo.GetUserByIdAsync(post.userId);
				try
				{
					using (SmtpClient client = new SmtpClient("smtp.gmail.com", 587))
					{
						client.EnableSsl = true;
						client.DeliveryMethod = SmtpDeliveryMethod.Network;
						client.UseDefaultCredentials = false;
						client.Credentials = new NetworkCredential(_config["Mail:Address"], _config["Mail:AppPassword"]);
						MailMessage msg = new MailMessage();
						msg.To.Add(admin.email);
						msg.From = new MailAddress(_config["Mail:Address"]);
						msg.Subject = "A new person has answered your PlayPost!";
						msg.Body = user.userName + " has answered to your post " + post.heading + ". Go chat with them!";
						client.Send(msg);
					}
				}
				catch (Exception ex) { }
			}

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
