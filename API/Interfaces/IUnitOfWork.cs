using System.Threading.Tasks;

namespace API.Interfaces
{
    public interface IUnitOfWork
    {
        IUserRepository UserRepository {get; }
        IMessageRepository MessageRepository {get;}
        ISampleRepository SampleRepository {get;}
        ILikesRepository LikesRepository {get; }
        Task<bool> Complete();
        bool HasChanges();
    }
}