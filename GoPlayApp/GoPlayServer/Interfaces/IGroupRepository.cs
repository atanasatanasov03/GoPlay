using GoPlayServer.Entities;

namespace GoPlayServer.Interfaces
{
    public interface IGroupRepository
    {
        void AddGroup(Group group);
        void UpdateGroup(Group group);
        Task<Group> GetGroupByName(string name);
        Task<IQueryable<AppUser>> GetUsersInGroup(string name);
        Task<IQueryable<Group>> GetGroupsForUser(string username);
        Task RemoveFromGroup(string username, string groupname);
    }
}
