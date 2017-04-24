using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Growth.BLL.DTO.Authorization;
using Growth.BLL.Infrastructure.Exceptions;
using Growth.BLL.Interfaces;
using Growth.DAL.Entities;
using Growth.DAL.Interfaces;
using Microsoft.Extensions.Logging;

namespace Growth.BLL.Services
{
    public class AccountService : IAccountService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IRoleService roleService;
        private readonly ICryptoProvider cryptoProvider;
        private readonly IMapper mapper;
        private readonly ILogger<AccountService> logger;

        public AccountService(
            IUnitOfWork unitOfWork, 
            IRoleService roleService, 
            ICryptoProvider cryptoProvider, 
            IMapper mapper, 
            ILogger<AccountService> logger)
        {
            this.unitOfWork = unitOfWork;
            this.roleService = roleService;
            this.cryptoProvider = cryptoProvider;
            this.mapper = mapper;
            this.logger = logger;
        }

        public UserDto Login(LoginModelDto loginModel)
        {
            var user = unitOfWork.Users.Find(u => u.Email.Equals(loginModel.Email)).FirstOrDefault();

            if (user == null || !cryptoProvider.VerifyHash(loginModel.Password, user.PasswordHash))
            {
                throw new ServiceException("Invalid password", "Password");
            }

            var userDto = mapper.Map<UserDto>(user);

            logger.LogInformation($"User login with email {loginModel.Email}");

            return userDto;
        }

        public async Task RegisterAsync(RegisterModelDto registerModelDto)
        {
            ValidateEmail(registerModelDto.Email);

            var user = mapper.Map<User>(registerModelDto);

            AppendDefaultRole(user);

            user.PasswordHash = cryptoProvider.GetHash(registerModelDto.Password);

            await unitOfWork.Users.CreateAsync(user);

            logger.LogInformation($"User register with email {registerModelDto.Email}");
        }

        private void ValidateEmail(string email)
        {
            if (unitOfWork.Users.Find(user => user.Email.Equals(email)).Any())
            {
                throw new EntityExistsException($"User with such email is already exists. Email: {email}", "Email");
            }
        }

        private void AppendDefaultRole(User user)
        {
            try
            {
                roleService.Get("user");
            }
            catch (EntityNotFoundException)
            {
                roleService.CreateAsync("user");
            }

            user.Roles.Add("user");
        }
    }
}