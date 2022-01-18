using GoPlayServer.Entities;
using GoPlayServer.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace GoPlayServer.Data
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;
        public UserRepository(AppDbContext context)
        {
            _context = context;
        }

        public void AddUser(RegularUser user)
        {
            _context.RegularUsers.Add(user);
        }

        public async Task<RegularUser> GetUserByIdAsync(int id)
        {
            return await _context.RegularUsers.FindAsync(id);
        }

        public async Task<RegularUser> GetUserByUsernameAsync(string username)
        {
            return await _context.RegularUsers.SingleOrDefaultAsync(x => x.userName == username);
        }

        public string GetUsernameByTokenAsync(string token)
        {
            string secret = "Nasko.DiplomnaRabota.Key";
            var key = Encoding.ASCII.GetBytes(secret);
            var handler = new JwtSecurityTokenHandler();
            var validations = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false
            };
            var claims = handler.ValidateToken(token, validations, out var tokenSecure);
            return claims.Identity.Name;
        }

        public async Task<IEnumerable<RegularUser>> GetUsersAsync()
        {
            return await _context.RegularUsers.ToListAsync();
        }

        public void Update(RegularUser user)
        {
            _context.Entry(user).State = EntityState.Modified;
        }

        public async Task<bool> SaveAllAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
