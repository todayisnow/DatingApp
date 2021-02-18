using System.Collections.Generic;
using System.Threading.Tasks;
using WebUi.DTOs;
using WebUi.Entities;
using WebUi.Helpers;

namespace WebUi.Interfaces
{
    public interface ILikesRepository
    {
        Task<UserLike> GetUserLike(int sourceUserId, int likedUserId);
        Task<AppUser> GetUserWithLikes(int userId);
        Task<PagedList<LikeDto>> GetUserLikes(LikesParams likesParams);
    }
}