using AutoMapper;
using Growth.BLL.DTO;
using Growth.BLL.DTO.Authorization;
using Growth.DAL.Entities;

namespace Growth.BLL.Infrastructure.Automapper
{
    public class DtoToEntityProfile : Profile
    {
        public DtoToEntityProfile()
        {
            CreateMap<UserDto, User>();
            CreateMap<RoleDto, Role>();
            CreateMap<RegisterModelDto, User>();
            CreateMap<KidDto, Kid>();
            CreateMap<PathDto, Path>();
            CreateMap<GoalDto, Goal>();
            CreateMap<StepDto, Step>();
        }
    }
}