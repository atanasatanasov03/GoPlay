using GoPlayServer.Entities;

namespace GoPlayServer.Interfaces
{
    public interface IGroupRepository
    {
        Task AddGroupAsync(Group group);
        void RemoveGroup(Group group);
        void UpdateGroup(Group group);
        Task<Group> GetGroupByNameAsync(string name);
        Task<Group> GetGroupByIdAsync(Guid? id);
        Task<string> UserInGroup(string username, string groupname);
        Task<IQueryable<AppUser>> GetUsersInGroup(string name);
        Task<IQueryable<Group>> GetGroupsForUser(string username);
        Task RemoveFromGroup(string username, string groupname);
    }
}
