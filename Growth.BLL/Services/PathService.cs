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
    public class PathService : IPathService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public PathService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<PathDto>> GetAllAsync(Guid kidId)
        {
            var paths = await _unitOfWork.Paths.GetByKidAsync(kidId);
            var pathDtos = _mapper.Map<IEnumerable<PathDto>>(paths);

            return pathDtos;
        }

        public async Task<PathDto> GetAsync(Guid kidId, Guid pathId)
        {
            var path = await _unitOfWork.Paths.GetAsync(kidId, pathId);

            if (path == null)
            {
                throw new EntityNotFoundException(
                    $"Cannot find path with such id. Kid id: {kidId}. Paht id: {pathId}",
                    "Path");
            }

            var pathDto = _mapper.Map<PathDto>(path);

            return pathDto;
        }

        public async Task<Guid> CreateAsync(Guid kidId, PathDto pathDto)
        {
            var kid = await _unitOfWork.Kids.GetAsync(kidId);
            if (kid == null)
            {
                throw new EntityNotFoundException(
                    $"Cannot find kid to create path. Kid id: {kidId}.",
                    "Kid");
            }

            var path = _mapper.Map<Path>(pathDto);
            var id = await _unitOfWork.Paths.CreateAsync(kidId, path);

            return id;
        }

        public async Task<Guid> UpdateAsync(Guid kidId, PathDto pathDto)
        {
            var kid = await _unitOfWork.Kids.GetAsync(kidId);
            if (kid == null)
            {
                throw new EntityNotFoundException(
                    $"Cannot find kid to update path. Kid id: {kidId}.",
                    "Kid");
            }

            var pathToUpdate = await _unitOfWork.Paths.GetAsync(kidId, pathDto.Id);
            if (pathToUpdate == null)
            {
                throw new EntityNotFoundException(
                    $"Cannot find path to update.Id: {pathDto.Id}.",
                    "Path");
            }

            var path = _mapper.Map<Path>(pathDto);
            var id = await _unitOfWork.Paths.UpdateAsync(path);

            return id;
        }

        public async Task DeleteAsync(Guid kidId, Guid pathId)
        {
            var path = await _unitOfWork.Paths.GetAsync(kidId, pathId);

            if (path == null)
            {
                throw new EntityNotFoundException(
                    $"Cannot find path with such id. kid id: {kidId}. Path id: {pathId}",
                    "Kid");
            }

            await _unitOfWork.Paths.DeleteAsync(pathId);
        }
    }
}
