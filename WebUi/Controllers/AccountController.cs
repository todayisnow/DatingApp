using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using WebUi.Data;
using WebUi.Dto;
using WebUi.Entities;
using WebUi.Interfaces;

namespace WebUi.Controllers
{

    public class AccountController : BaseApiController
    {
        private readonly DataContext _context;


        private readonly ITokenService _tokenService;

        public AccountController(DataContext context, ITokenService tokenService)
        {
            _context = context;
            _tokenService = tokenService;
        }
        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
        {
            if (await UserExits(registerDto.Username)) return BadRequest("Username is taken!");

            using var hmac = new HMACSHA512();

            var user = new AppUser
            {
                UserName = registerDto.Username.ToLower(),
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password)),
                PasswordSalt = hmac.Key
            };
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            return new UserDto
            {
                Username = registerDto.Username.ToLower(),
                Token = _tokenService.CreateToken(user)
            };
        }
        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
            var user = await _context.Users.SingleOrDefaultAsync(x => x.UserName == loginDto.Username.ToLower());
            if (user != null)
            {
                using var hmac = new HMACSHA512(user.PasswordSalt);
                var ch = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));
                return !ch.SequenceEqual(user.PasswordHash) ? Unauthorized("Invalid Password") :  new UserDto
                {
                    Username = loginDto.Username.ToLower(),
                    Token = _tokenService.CreateToken(user)
                }; 
            }

            return Unauthorized("Invalid Username");
        }
        private async Task<bool> UserExits(string Username) => await _context.Users.AnyAsync(m => m.UserName == Username.ToLower());
        
    }
   
}
