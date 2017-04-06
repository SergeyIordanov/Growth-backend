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
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public KidService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<KidDto>> GetAllAsync(Guid userId)
        {
            var kids = await _unitOfWork.Kids.GetByUserAsync(userId);
            var kidDtos = _mapper.Map<IEnumerable<KidDto>>(kids);

            return kidDtos;
        }

        public async Task<KidDto> GetAsync(Guid userId, Guid kidId)
        {
            var kid = await _unitOfWork.Kids.GetAsync(userId, kidId);

            if (kid == null)
            {
                throw new EntityNotFoundException(
                    $"Cannot find kid with such id for current user. User id: {userId}. Kid id: {kidId}",
                    "Kid");
            }

            var kidDto = _mapper.Map<KidDto>(kid);

            return kidDto;
        }

        public async Task<Guid> CreateAsync(Guid userId, KidDto kidDto)
        {
            var user = await _unitOfWork.Users.GetAsync(userId);
            if (user == null)
            {
                throw new EntityNotFoundException(
                    $"Cannot find user to create kid. User id: {userId}.",
                    "User");
            }

            var kid = _mapper.Map<Kid>(kidDto);
            var id = await _unitOfWork.Kids.CreateAsync(userId, kid);

            return id;
        }

        public async Task DeleteAsync(Guid userId, Guid kidId)
        {
            var kid = await _unitOfWork.Kids.GetAsync(userId, kidId);

            if (kid == null)
            {
                throw new EntityNotFoundException(
                    $"Cannot find kid with such id for current user. User id: {userId}. Kid id: {kidId}",
                    "Kid");
            }

            await _unitOfWork.Kids.DeleteAsync(userId, kidId);
        }
    }
}