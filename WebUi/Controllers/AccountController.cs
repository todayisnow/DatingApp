using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
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
        private readonly IMapper _mapper;

        public AccountController(DataContext context, ITokenService tokenService, IMapper mapper)
        {
            _context = context;
            _tokenService = tokenService;
            _mapper = mapper;
        }
        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
        {
            if (await UserExits(registerDto.Username)) return BadRequest("Username is taken!");
            var user = _mapper.Map<AppUser>(registerDto);
            using var hmac = new HMACSHA512();



            user.PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password));
            user.PasswordSalt = hmac.Key;

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            return new UserDto
            {
                Username = registerDto.Username.ToLower(),
                Token = _tokenService.CreateToken(user),
                KnownAs = user.KnownAs,
                Gender = user.Gender
            };
        }
        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
            var user = await _context.Users.Include(x => x.Photos).SingleOrDefaultAsync(x => x.UserName == loginDto.Username.ToLower());
            if (user != null)
            {
                using var hmac = new HMACSHA512(user.PasswordSalt);
                var ch = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));
                return !ch.SequenceEqual(user.PasswordHash) ? Unauthorized("Invalid Password") : new UserDto
                {
                    Username = loginDto.Username.ToLower(),
                    Token = _tokenService.CreateToken(user),
                    PhotoUrl = user.Photos.FirstOrDefault(m => m.IsMain)?.Url,
                    KnownAs = user.KnownAs,
                    Gender = user.Gender



                };
            }

            return Unauthorized("Invalid Username");
        }
        private async Task<bool> UserExits(string Username) => await _context.Users.AnyAsync(m => m.UserName == Username.ToLower());

    }

}
