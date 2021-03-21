using System.Threading.Tasks;

namespace WebUi1.Interfaces
{
    public interface IUnitOfWork
    {
        IUserRepository UserRepository {get; }
        IMessageRepository MessageRepository {get;}
        ILikesRepository LikesRepository {get; }
        Task<bool> Complete();
        bool HasChanges();
    }
}