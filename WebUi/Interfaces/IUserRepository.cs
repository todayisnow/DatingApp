using System.Collections.Generic;
using System.Threading.Tasks;
using WebUi.Dto;
using WebUi.Entities;
using WebUi.Helpers;
using WebUi.Entities;

namespace WebUi.Interfaces
{
    public interface IUserRepository
    {
        void Update(AppUser user);
        Task<IEnumerable<AppUser>> GetUsersAsync();
        Task<AppUser> GetUserByIdAsync(int id);
        Task<AppUser> GetUserByUsernameAsync(string username);
        Task<IEnumerable<MemberDto>> GetMembersAsync();
        Task<MemberDto> GetMemberAsync(string username);
        Task<string> GetUserGender(string username);
        Task<bool> SaveAllAsync();
    }
}