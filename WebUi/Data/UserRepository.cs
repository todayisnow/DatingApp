using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using WebUi.Dto;
using WebUi.Entities;
//using WebUi.Helpers;
using WebUi.Interfaces;
//using AutoMapper;
//using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using WebUi.Helpers;

namespace WebUi.Data
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        public UserRepository(DataContext context, IMapper mapper)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<MemberDto> GetMemberAsync(string username)
        {
            return await _context.Users
                .Where(x => x.UserName == username)
                .ProjectTo<MemberDto>(_mapper.ConfigurationProvider)
                .SingleOrDefaultAsync();
        }

        public async Task<PagedList<MemberDto>> GetMembersAsync(UserParams userParams)
        {
            //return await _context.Users.ProjectTo<MemberDto>(_mapper.ConfigurationProvider)
            //    .ToListAsync();
            var query = _context.Users.ProjectTo<MemberDto>(_mapper.ConfigurationProvider).AsNoTracking();
            return await PagedList<MemberDto>.CreateAsync(query, userParams.PageNumber, userParams.PageSize);
            //query = query.Where(u => u.UserName != userParams.CurrentUsername);
            //query = query.Where(u => u.Gender == userParams.Gender);

            //var minDob = DateTime.Today.AddYears(-userParams.MaxAge - 1);
            //var maxDob = DateTime.Today.AddYears(-userParams.MinAge);

            //query = query.Where(u => u.DateOfBirth >= minDob && u.DateOfBirth <= maxDob);

            //query = userParams.OrderBy switch
            //{
            //    "created" => query.OrderByDescending(u => u.Created),
            //    _ => query.OrderByDescending(u => u.LastActive)
            //};

            //return await PagedList<MemberDto>.CreateAsync(query.ProjectTo<MemberDto>(_mapper
            //    .ConfigurationProvider).AsNoTracking(),
            //        userParams.PageNumber, userParams.PageSize);
        }

        public async Task<AppUser> GetUserByIdAsync(int id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task<AppUser> GetUserByUsernameAsync(string username)
        {
            return await _context.Users
                .Include(p => p.Photos)
                .SingleOrDefaultAsync(x => x.UserName == username);
        }

        public async Task<string> GetUserGender(string username)
        {
            return await _context.Users
                .Where(x => x.UserName == username)
                .Select(x => x.Gender).FirstOrDefaultAsync();
        }

        public async Task<bool> SaveAllAsync()
        {
            var result = await _context.SaveChangesAsync();
            return result>0 ;
        }

        public async Task<IEnumerable<AppUser>> GetUsersAsync()
        {
            return await _context.Users
                .Include(p => p.Photos)
                .ToListAsync();
        }

        public void Update(AppUser user)
        {
            _context.Entry(user).State = EntityState.Modified;
        }
    }
}