using GoPlayServer.Entities;

namespace GoPlayServer.Interfaces
{
    public interface IUserRepository
    {
        void AddUser(AppUser user);
        void Update(AppUser user);
        public void Delete(AppUser user);
        Task<IEnumerable<AppUser>> GetUsersAsync();
        Task<IEnumerable<AppUser>> GetUsersbyRoleAsync(string role);
        Task<AppUser> GetUserByIdAsync(Guid id);
        Task<AppUser> GetUserByUsernameAsync(string username);
        string GetUsernameByTokenAsync(string token);
    }
}
