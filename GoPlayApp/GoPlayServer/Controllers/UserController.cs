using GoPlayServer.Data;
using GoPlayServer.DTOs;
using GoPlayServer.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace GoPlayServer.Controllers
{
    [Route("users")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly AppDbContext _context;
        public UserController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost("register")]
        public async Task<ActionResult<RegularUserDTO>> register(RegisterUserDTO RegisterUserDTO)
        {
            if (await UserExists(RegisterUserDTO.userName)) return new BadRequestResult();


            using var hmac = new HMACSHA512();

            var user = new RegularUser
            {
                userName = RegisterUserDTO.userName.ToLower(),
                firstName = RegisterUserDTO.firstName,
                lastName = RegisterUserDTO.lastName,
                address = RegisterUserDTO.address,
                passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(RegisterUserDTO.password)),
                passwordSalt = hmac.Key
            };

            _context.RegularUsers.Add(user);
            await _context.SaveChangesAsync();

            return new RegularUserDTO
            {
                userName = user.userName,
                firstName = user.firstName,
                lastName = user.lastName,
                token = CreateToken(user)
            };
        }

        [HttpPost("login")]
        public async Task<ActionResult<RegularUserDTO>> login(LoginDTO loginDto)
        {
            var user = await _context.RegularUsers.SingleOrDefaultAsync(user => user.userName == loginDto.username);

            if (user == null) return new UnauthorizedResult();

            using var hmac = new HMACSHA512(user.passwordSalt);

            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.password));

            for (int i = 0; i < computedHash.Length; i++)
            {
                if (computedHash[i] != user.passwordHash[i]) return new UnauthorizedResult();
            }

            return new RegularUserDTO
            {
                userName = user.userName,
                firstName = user.firstName,
                lastName = user.lastName,
                token = CreateToken(user)
            };
        }

        public string CreateToken(RegularUser user)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.NameId, user.userName)
            };
            var _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("GoPlay Sports Platform"));
            var creds = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(7),
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }

        public async Task<bool> UserExists(string username)
        {
            return await _context.RegularUsers.AnyAsync(x => x.userName == username.ToLower());
        }
    }
}
