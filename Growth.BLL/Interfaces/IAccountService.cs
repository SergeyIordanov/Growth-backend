using System.Threading.Tasks;
using Growth.BLL.DTO.Authorization;

namespace Growth.BLL.Interfaces
{
    public interface IAccountService
    {
        UserDto Login(LoginModelDto loginModelDto);

        Task RegisterAsync(RegisterModelDto registerModelDto);
    }
}