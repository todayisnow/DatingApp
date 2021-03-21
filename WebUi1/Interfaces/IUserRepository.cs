using System.Collections.Generic;
using System.Threading.Tasks;
using WebUi1.Dto;
using WebUi1.Entities;
using WebUi1.Helpers;

namespace WebUi1.Interfaces
{
    public interface IUserRepository
    {
        void Update(AppUser user);
        Task<IEnumerable<AppUser>> GetUsersAsync();
        Task<AppUser> GetUserByIdAsync(int id);
        Task<AppUser> GetUserByUsernameAsync(string username);
        Task<PagedList<MemberDto>> GetMembersAsync(UserParams userParams);
        Task<MemberDto> GetMemberAsync(string username);
        Task<string> GetUserGenderAsync(string username);
        
    }
}