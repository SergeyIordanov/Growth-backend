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
    public class StepService : IStepService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public StepService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<StepDto>> GetAllAsync(Guid kidId, Guid pathId, Guid goalId)
        {
            var path = await _unitOfWork.Paths.GetAsync(kidId, pathId);

            if (path == null)
            {
                return new List<StepDto>();
            }

            var steps = await _unitOfWork.Steps.GetByGoalAsync(goalId);
            var stepDtos = _mapper.Map<IEnumerable<StepDto>>(steps);

            return stepDtos;
        }

        public async Task<StepDto> GetAsync(Guid kidId, Guid pathId, Guid goalId, Guid stepId)
        {
            var path = await _unitOfWork.Paths.GetAsync(kidId, pathId);
            var step = await _unitOfWork.Steps.GetAsync(goalId, stepId);

            if (path == null || step == null)
            {
                throw new EntityNotFoundException(
                    $"Cannot find step with such id. Kid id: {kidId}. Paht id: {pathId}. Goal id: {goalId}. Step id {stepId}",
                    "Kid");
            }

            var stepDto = _mapper.Map<StepDto>(step);

            return stepDto;
        }

        public async Task<Guid> CreateAsync(Guid kidId, Guid pathId, Guid goalId, StepDto stepDto)
        {
            var path = await _unitOfWork.Paths.GetAsync(kidId, pathId);
            var goal = await _unitOfWork.Goals.GetAsync(pathId, goalId);
            if (path == null || goal == null)
            {
                throw new EntityNotFoundException(
                    $"Cannot find goal to create step. Kid id: {kidId}. Path id: {pathId}. Goal id: {goalId}",
                    "Goal");
            }

            var step = _mapper.Map<Step>(stepDto);
            var id = await _unitOfWork.Steps.CreateAsync(goalId, step);

            return id;
        }

        public async Task<Guid> UpdateAsync(Guid kidId, Guid pathId, Guid goalId, StepDto stepDto)
        {
            var path = await _unitOfWork.Paths.GetAsync(kidId, pathId);
            if (path == null)
            {
                throw new EntityNotFoundException(
                    $"Cannot find path to update step. Kid id: {kidId}. Path id: {pathId}",
                    "Path");
            }

            var stepToUpdate = await _unitOfWork.Goals.GetAsync(goalId, stepDto.Id);
            if (stepToUpdate == null)
            {
                throw new EntityNotFoundException(
                    $"Cannot find step to update. Goal id: {goalId} Id: {stepDto.Id}.",
                    "Step");
            }

            var step = _mapper.Map<Step>(stepDto);
            var id = await _unitOfWork.Steps.UpdateAsync(goalId, step);

            return id;
        }

        public async Task DeleteAsync(Guid kidId, Guid pathId, Guid goalId, Guid stepId)
        {
            var path = await _unitOfWork.Paths.GetAsync(kidId, pathId);

            if (path == null)
            {
                throw new EntityNotFoundException(
                    $"Cannot find path to delete the step in. Kid id: {kidId}. Path id: {pathId}",
                    "Path");
            }

            var step = await _unitOfWork.Steps.GetAsync(goalId, stepId);

            if (step == null)
            {
                throw new EntityNotFoundException(
                    $"Cannot find step to delete. Goal id: {goalId}. Step id: {stepId}",
                    "Step");
            }

            await _unitOfWork.Steps.DeleteAsync(stepId);
        }
    }
}
