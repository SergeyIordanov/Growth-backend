using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Growth.BLL.DTO;
using Growth.BLL.Infrastructure.Exceptions;
using Growth.BLL.Interfaces;
using Growth.DAL.Entities;
using Growth.DAL.Interfaces;

namespace Growth.BLL.Services
{
    public class KidService : IKidService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public KidService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        public async Task<IEnumerable<KidDto>> GetAllAsync(Guid userId)
        {
            var kids = await unitOfWork.Kids.GetByUserAsync(userId);
            var kidDtos = mapper.Map<IEnumerable<KidDto>>(kids);

            return kidDtos;
        }

        public async Task<KidDto> GetAsync(Guid userId, Guid kidId)
        {
            var kid = await unitOfWork.Kids.GetAsync(kidId);

            if (kid == null || kid.UserId != userId)
            {
                throw new EntityNotFoundException(
                    $"Cannot find kid with such id for current user. User id: {userId}. Kid id: {kidId}",
                    "Kid");
            }

            var kidDto = mapper.Map<KidDto>(kid);

            return kidDto;
        }

        public async Task<Guid> CreateAsync(Guid userId, KidDto kidDto)
        {
            var user = await unitOfWork.Users.GetAsync(userId);
            if (user == null)
            {
                throw new EntityNotFoundException(
                    $"Cannot find user to create kid. User id: {userId}.",
                    "User");
            }

            var kid = mapper.Map<Kid>(kidDto);
            kid.UserId = userId;

            var id = await unitOfWork.Kids.CreateAsync(kid);

            return id;
        }

        public async Task DeleteAsync(Guid userId, Guid kidId)
        {
            var kid = await unitOfWork.Kids.GetAsync(kidId);

            if (kid == null || kid.UserId != userId)
            {
                throw new EntityNotFoundException(
                    $"Cannot find kid with such id for current user. User id: {userId}. Kid id: {kidId}",
                    "Kid");
            }

            await unitOfWork.Kids.DeleteAsync(kidId);
        }
    }
}