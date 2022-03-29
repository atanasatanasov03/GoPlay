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
using System.Web.Http.Cors;

namespace GoPlayServer.Controllers
{
    [Route("users")]
    [ApiController]
    //[Authorize]
    public class UserController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IUserRepository _userRepo;
        
        public UserController(AppDbContext context, IUserRepository userRepo)
        {
            _context = context;
            _userRepo = userRepo;
        }

        [AllowAnonymous]
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
                sports = RegisterUserDTO.sports
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
                token = _userRepo.GenerateJwtToken(user)
            };
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ActionResult<AppUserDTO>> login(LoginDTO loginDto)
        {
            if(!await UserExists(loginDto.username)) return new UnauthorizedResult();

            AppUserDTO userDto = await _userRepo.Authenticate(loginDto);

            if (userDto is null) return Unauthorized("Bad credentials");

            return Ok(userDto);
        }

        [HttpPost("muteUser")]
        public async Task<ActionResult> MuteUser(string username, int period)
        {
            Request.Headers.TryGetValue("Authorization", out var header);
            if (header.IsNullOrEmpty()) return new UnauthorizedResult();
            var token = header.First().Split(" ").Last();

            if (_userRepo.ValidateToken(token).IsNullOrEmpty()) return new UnauthorizedResult();

            var user = await _userRepo.GetUserByUsernameAsync(username);

            if (user == null) return new BadRequestResult();

            user.mutedOn = DateTime.Now;
            user.mutedFor = period;

            _userRepo.Update(user);
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpPost("banUser")]
        public async Task<ActionResult> banUser(string username)
        {
            Request.Headers.TryGetValue("Authorization", out var header);
            if (header.IsNullOrEmpty()) return new UnauthorizedResult();
            var token = header.First().Split(" ").Last();

            if (_userRepo.ValidateToken(token).IsNullOrEmpty()) return new UnauthorizedResult();

            var user = await _userRepo.GetUserByUsernameAsync(username);

            if (user == null) return new BadRequestResult();

            user.banned = true;

            _userRepo.Update(user);
            await _context.SaveChangesAsync();

            return Ok();
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
            Request.Headers.TryGetValue("Authorization", out var header);
            if(header.IsNullOrEmpty()) return new UnauthorizedResult();
            var token = header.First().Split(" ").Last();

            if(_userRepo.ValidateToken(token).IsNullOrEmpty()) return new UnauthorizedResult();

            var user = await _userRepo.GetUserByUsernameAsync(username.ToLower());

            if (user == null) return new BadRequestResult();

            return token;
        }

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

        public async Task<bool> UserExists(string username)
        {
            return await _context.AppUsers.AnyAsync(x => x.userName == username.ToLower());
        }
    }
}
