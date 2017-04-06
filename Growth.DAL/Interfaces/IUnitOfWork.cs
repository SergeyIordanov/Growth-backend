using Growth.DAL.Entities;

namespace Growth.DAL.Interfaces
{
    public interface IUnitOfWork
    {
        IRepository<User> Users { get; }

        IRepository<Role> Roles { get; }

        IKidRepository Kids { get; }

        IPathRepository Paths { get; }
    }
}