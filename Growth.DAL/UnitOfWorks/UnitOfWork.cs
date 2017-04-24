using System;
using Growth.DAL.Entities;
using Growth.DAL.Interfaces;
using Growth.DAL.Repositories;

namespace Growth.DAL.UnitOfWorks
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly Lazy<IRepository<User>> userRepository;

        private readonly Lazy<IRepository<Role>> roleRepository;

        private readonly Lazy<IKidRepository> kidRepository;

        private readonly Lazy<IPathRepository> pathRepository;

        private readonly Lazy<IGoalRepository> goalRepository;

        private readonly Lazy<IStepRepository> stepRepository;

        public UnitOfWork(IDbContext context)
        {
            var db = context;
            userRepository = new Lazy<IRepository<User>>(() => new CommonRepository<User>(db));
            roleRepository = new Lazy<IRepository<Role>>(() => new CommonRepository<Role>(db));
            kidRepository = new Lazy<IKidRepository>(() => new KidRepository(db));
            pathRepository = new Lazy<IPathRepository>(() => new PathRepository(db));
            goalRepository = new Lazy<IGoalRepository>(() => new GoalRepository(db));
            stepRepository = new Lazy<IStepRepository>(() => new StepRepository(db));
        }

        public IRepository<User> Users => userRepository.Value;

        public IRepository<Role> Roles => roleRepository.Value;

        public IKidRepository Kids => kidRepository.Value;

        public IPathRepository Paths => pathRepository.Value;

        public IGoalRepository Goals => goalRepository.Value;

        public IStepRepository Steps => stepRepository.Value;
    }
}