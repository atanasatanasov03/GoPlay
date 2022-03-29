using GoPlayServer.DTOs;
using GoPlayServer.Entities;

namespace GoPlayServer.Interfaces
{
    public interface IMessageRepository
    {
        void SaveMessage(Message message);
        void RemoveMessagesFor(Guid groupId);
        ICollection<MessageDTO> GetMessagesIn(string groupname);
    }
}
