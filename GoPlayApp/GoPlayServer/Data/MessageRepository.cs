using GoPlayServer.DTOs;
using GoPlayServer.Entities;
using GoPlayServer.Interfaces;

namespace GoPlayServer.Data
{
    public class MessageRepository : IMessageRepository
    {
        private readonly AppDbContext _context;
        public MessageRepository(AppDbContext context)
        {
            _context = context;
        }

        public ICollection<MessageDTO> GetMessagesIn(string groupname)
        {
            var group = _context.Groups.SingleOrDefault(g => g.groupName == groupname);

            if (group == null) return null;

            IEnumerable<Message> messages = _context.Message.ToList().Where(m => m.GroupId == group.Id);
            ICollection<MessageDTO> messageDTOs = new List<MessageDTO>();

            foreach(var message in messages)
            {
                messageDTOs.Add(new MessageDTO { 
                    text = message.text,
                    username = message.userName,
                    dateTime = message.date,
                    groupName = groupname
                });
            }

            return messageDTOs;
            
        }

        public void SaveMessage(Message message) => _context.Message.Add(message);
        
    }
}
