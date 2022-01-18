using GoPlayServer.Entities;

namespace GoPlayServer.Interfaces
{
    public interface IUserRepository
    {
        void Update(RegularUser user);
        Task<IEnumerable<RegularUser>> GetUsersAsync();
        Task<RegularUser> GetUserByIdAsync(int id);
        Task<RegularUser> GetUserByUsernameAsync(string username);
        string GetUsernameByTokenAsync(string token);
        void AddUser(RegularUser user);
    }
}
