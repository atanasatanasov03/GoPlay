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
using Newtonsoft.Json;
using GoPlayServer.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace GoPlayServer.Controllers
{
    [Route("users")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IUserRepository _userRepo;
        
        public UserController(AppDbContext context, IUserRepository userRepo)
        {
            _context = context;
            _userRepo = userRepo;
        }

        [HttpPost("register")]
        public async Task<ActionResult<AppUserDTO>> register(RegisterUserDTO RegisterUserDTO)
        {
            if (await UserExists(RegisterUserDTO.userName)) return new BadRequestResult();

            using var hmac = new HMACSHA512();

            var user = new AppUser
            {
                userName = RegisterUserDTO.userName.ToLower(),
                firstName = RegisterUserDTO.firstName,
                lastName = RegisterUserDTO.lastName,
                address = RegisterUserDTO.address,
                role = RegisterUserDTO.role,
                email = RegisterUserDTO.email,
                age = RegisterUserDTO.age,
                passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(RegisterUserDTO.password)),
                passwordSalt = hmac.Key,
                sports = ""
            };

            _userRepo.AddUser(user);
            await _context.SaveChangesAsync();

            return new AppUserDTO
            {
                userName = user.userName,
                role = user.role,
                firstName = user.firstName,
                lastName = user.lastName,
                email = user.email,
                token = CreateToken(user)
            };
        }

        [HttpPost("login")]
        public async Task<ActionResult<AppUserDTO>> login(LoginDTO loginDto)
        {
            if(!await UserExists(loginDto.username)) return new UnauthorizedResult();

            var user = await _userRepo.GetUserByUsernameAsync(loginDto.username.ToLower());

            using var hmac = new HMACSHA512(user.passwordSalt);

            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.password));

            for (int i = 0; i < computedHash.Length; i++)
            {
                if (computedHash[i] != user.passwordHash[i]) return new UnauthorizedResult();
            }

            return new AppUserDTO
            {
                userName = user.userName,
                role = user.role,
                firstName = user.firstName,
                lastName = user.lastName,
                email = user.email,
                token = CreateToken(user)
            };
        }

        [HttpPost("setSports")]
        public async Task<ActionResult<string>> setSports(setSportsDTO setSports)
        {
            var user = await _userRepo.GetUserByUsernameAsync(setSports.username.ToLower());

            if (user == null) return new BadRequestResult();

            user.sports = setSports.sports;
            _userRepo.Update(user);
            await _context.SaveChangesAsync();

            return setSports.sports;
        }

        [HttpGet("sports")]
        public async Task<ActionResult<string>> getSports(string username)
        {
            var user = await _userRepo.GetUserByUsernameAsync(username.ToLower());

            if (user == null) return new BadRequestResult();

            return user.sports;
        }

        [Authorize]
        [HttpGet("getUser")]
        public async Task<ActionResult<AppUserDTO>> GetUserByUsername(string username)
        {
            var user = await _userRepo.GetUserByUsernameAsync(username);

            if (user == null) return new BadRequestResult();

            return Ok(new AppUserDTO
            {
                userName = username,
                firstName = user.firstName,
                lastName = user.lastName,
                email = user.email,
                role = user.role
            });
        }

        public string CreateToken(AppUser user)
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
            return await _context.AppUsers.AnyAsync(x => x.userName == username.ToLower());
        }
    }
}
