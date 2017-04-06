using AutoMapper;
using Growth.BLL.DTO;
using Growth.BLL.DTO.Authorization;
using Growth.WEB.Models;
using Growth.WEB.Models.AccountApiModels;

namespace Growth.WEB.Infrastructure.Automapper
{
    /// <summary>
    /// AutoMapper profile from Dto model to ApiModels
    /// </summary>
    public class DtoToApiModelProfile : Profile
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public DtoToApiModelProfile()
        {
            CreateMap<UserDto, UserApiModel>();
            CreateMap<RoleDto, RoleApiModel>();
            CreateMap<RegisterModelDto, RegisterApiModel>();
            CreateMap<LoginModelDto, LoginApiModel>();
            CreateMap<KidDto, KidApiModel>();
            CreateMap<PathDto, PathApiModel>();
            CreateMap<GoalDto, GoalApiModel>();
            CreateMap<StepDto, StepApiModel>();
        }
    }
}