using GoPlayServer.Entities;
using GoPlayServer.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GoPlayServer.Data
{
    public class GroupRepository : IGroupRepository
    {
        private readonly AppDbContext _context;

        public GroupRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddGroupAsync(Group group)
        {
            await _context.Groups.AddAsync(group);
        }

        public void RemoveGroup(Group group) {
            _context.Groups.Remove(group);
        }

        public async Task<Group> GetGroupByNameAsync(string name)
        {
            return await _context.Groups.SingleOrDefaultAsync(g => g.groupName == name);
        }
        public async Task<Group> GetGroupByIdAsync(Guid? id)
        {
            return await _context.Groups.SingleOrDefaultAsync(g => g.Id == id);
        }

        public void UpdateGroup(Group group)
        {
            _context.Entry(group).State = EntityState.Modified;
        }

        public async Task<IQueryable<AppUser>> GetUsersInGroup(string name)
        {
            Group groupp = await GetGroupByNameAsync(name);

            var usersQuery = from g in _context.Groups
                             from u in g.users
                             where g.Id == groupp.Id
                             select u;

            return usersQuery;
        }

        public async Task<IQueryable<Group>> GetGroupsForUser(string username)
        {
            AppUser user = await _context.AppUsers.SingleOrDefaultAsync(u => u.userName == username);

            if (user is null) return null;

            var groupsQuery = from u in _context.AppUsers
                              from g in u.groups
                              where u.Id == user.Id
                              select g;

            return groupsQuery;
        }

        public async Task RemoveFromGroup(string username, string groupname)
        {
            var group = await _context.Groups.Include(g => g.users).SingleOrDefaultAsync(g => g.groupName == groupname);
            var user = await _context.AppUsers.SingleOrDefaultAsync(u => u.userName == username);

            if (user is null || group is null) return;

            group.users.Remove(user);
            await _context.SaveChangesAsync();
        }
    }
}
