using System.Collections.Generic;
using System.Threading.Tasks;
using WebUi1.Dto;
using WebUi1.Entities;
using WebUi1.Helpers;

namespace WebUi1.Interfaces
{
    public interface ILikesRepository
    {
        Task<UserLike> GetUserLike(int sourceUserId, int likedUserId);
        Task<AppUser> GetUserWithLikes(int userId);
        Task<PagedList<LikeDto>> GetUserLikes(LikesParams likesParams);
    }
}