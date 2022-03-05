using GoPlayServer.DTOs;
using GoPlayServer.Entities;

namespace GoPlayServer.Interfaces
{
    public interface IMessageRepository
    {
        void SaveMessage(Message message);
        ICollection<MessageDTO> GetMessagesIn(string groupname);
    }
}
