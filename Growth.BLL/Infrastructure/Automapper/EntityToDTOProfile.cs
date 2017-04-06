using AutoMapper;
using Growth.BLL.DTO;
using Growth.BLL.DTO.Authorization;
using Growth.DAL.Entities;

namespace Growth.BLL.Infrastructure.Automapper
{
    public class EntityToDtoProfile : Profile
    {
        public EntityToDtoProfile()
        {
            CreateMap<User, UserDto>();
            CreateMap<Role, RoleDto>();
            CreateMap<Kid, KidDto>();
            CreateMap<Path, PathDto>();
            CreateMap<Goal, GoalDto>();
            CreateMap<Step, StepDto>();
        }
    }
}