using GoPlayServer.Entities;
using GoPlayServer.Interfaces;

namespace GoPlayServer.Data
{
    public class GroupRepository : IGroupRepository
    {
        private readonly AppDbContext _context;

        public GroupRepository(AppDbContext context)
        {
            _context = context;
        }

        public void AddGroup(Group group) => _context.Groups.Add(group);
    }
}
