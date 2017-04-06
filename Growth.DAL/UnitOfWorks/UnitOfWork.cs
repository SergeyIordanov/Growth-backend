using System;
using Growth.DAL.Entities;
using Growth.DAL.Interfaces;
using Growth.DAL.Repositories;

namespace Growth.DAL.UnitOfWorks
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly Lazy<IRepository<User>> _userRepository;

        private readonly Lazy<IRepository<Role>> _roleRepository;

        private readonly Lazy<IKidRepository> _kidRepository;

        private readonly Lazy<IPathRepository> _pathRepository;

        public UnitOfWork(IDbContext context)
        {
            var db = context;
            _userRepository = new Lazy<IRepository<User>>(() => new CommonRepository<User>(db));
            _roleRepository = new Lazy<IRepository<Role>>(() => new CommonRepository<Role>(db));
            _kidRepository = new Lazy<IKidRepository>(() => new KidRepository(db));
            _pathRepository = new Lazy<IPathRepository>(() => new PathRepository(db));
        }

        public IRepository<User> Users => _userRepository.Value;

        public IRepository<Role> Roles => _roleRepository.Value;

        public IKidRepository Kids => _kidRepository.Value;

        public IPathRepository Paths => _pathRepository.Value;
    }
}