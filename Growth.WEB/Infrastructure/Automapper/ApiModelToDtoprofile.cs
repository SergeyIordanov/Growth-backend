using AutoMapper;
using Growth.BLL.DTO;
using Growth.BLL.DTO.Authorization;
using Growth.WEB.Models;
using Growth.WEB.Models.AccountApiModels;

namespace Growth.WEB.Infrastructure.Automapper
{
    /// <summary>
    /// AutoMapper profile from ApiModels to Dto model
    /// </summary>
    public class ApiModelToDtoProfile : Profile
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public ApiModelToDtoProfile()
        {
            CreateMap<UserApiModel, UserDto>();
            CreateMap<RoleApiModel, RoleDto>();
            CreateMap<LoginApiModel, LoginModelDto>();
            CreateMap<RegisterApiModel, RegisterModelDto>();
            CreateMap<KidApiModel, KidDto>();
            CreateMap<PathApiModel, PathDto>();
            CreateMap<GoalApiModel, GoalDto>();
            CreateMap<StepApiModel, StepDto>();
        }
    }
}