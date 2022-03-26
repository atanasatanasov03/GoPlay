﻿using GoPlayServer.DTOs;
using GoPlayServer.Entities;
using GoPlayServer.Helpers;
using GoPlayServer.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace GoPlayServer.Data
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _config;
        public UserRepository(AppDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        public void AddUser(AppUser user)
        {
            _context.AppUsers.Add(user);
        }

        public void Update(AppUser user)
        {
            _context.Entry(user).State = EntityState.Modified;
        }

        public void Delete(AppUser user)
        {
            _context.AppUsers.Remove(user);
        }

        public async Task<AppUser> GetUserByIdAsync(Guid id)
        {
            return await _context.AppUsers.FindAsync(id);
        }

        public async Task<AppUser> GetUserByUsernameAsync(string username)
        {
            return await _context.AppUsers.SingleOrDefaultAsync(x => x.userName == username);
        }

        public string GetUsernameByTokenAsync(string token)
        {
            
            var key = Encoding.ASCII.GetBytes(_config["Jwt:TokenKey"]);
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

        public async Task<IEnumerable<AppUser>> GetUsersAsync()
        {
            return await _context.AppUsers.ToListAsync();
        }

        public async Task<bool> SaveAllAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<IEnumerable<AppUser>> GetUsersbyRoleAsync(string role)
        {
            return await _context.AppUsers.Where(u => u.role == role).ToListAsync();
        }
        
        public string GenerateJwtToken(AppUser user)
        {
            // generate token that is valid for 7 days
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:TokenKey"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
              _config["Jwt:Audience"],
              null,
              expires: DateTime.Now.AddMinutes(120),
              signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<AppUserDTO> Authenticate(LoginDTO loginDto)
        {
            var user = await GetUserByUsernameAsync(loginDto.username.ToLower());

            using var hmac = new HMACSHA512(user.passwordSalt);

            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.password));

            for (int i = 0; i < computedHash.Length; i++)
            {
                if (computedHash[i] != user.passwordHash[i]) return null;
            }

            if (user.mutedOn.HasValue)
            {
                DateTime muted = (DateTime)user.mutedOn;
                if (DateTime.Now >= muted.AddDays((double)user.mutedFor))
                {
                    user.mutedOn = null;
                    user.mutedFor = null;
                }
            }

            return new AppUserDTO
            {
                userName = user.userName,
                role = user.role,
                firstName = user.firstName,
                lastName = user.lastName,
                email = user.email,
                token = GenerateJwtToken(user),
                mutedOn = user.mutedOn,
                mutedFor = user.mutedFor,
                banned = user.banned
            };
        }
    }
}
