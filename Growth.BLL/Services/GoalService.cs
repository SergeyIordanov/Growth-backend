﻿using System;
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
    public class GoalService : IGoalService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public GoalService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        public async Task<IEnumerable<GoalDto>> GetAllAsync(Guid kidId, Guid pathId)
        {
            var kid = await unitOfWork.Kids.GetAsync(kidId);

            if (kid == null)
            {
                return new List<GoalDto>();
            }

            var goals = await unitOfWork.Goals.GetByPathAsync(pathId);
            var goalDtos = mapper.Map<IEnumerable<GoalDto>>(goals);

            return goalDtos;
        }

        public async Task<GoalDto> GetAsync(Guid kidId, Guid pathId, Guid goalId)
        {
            var kid = await unitOfWork.Kids.GetAsync(kidId);
            var goal = await unitOfWork.Goals.GetAsync(pathId, goalId);

            if (kid == null || goal == null)
            {
                throw new EntityNotFoundException(
                    $"Cannot find goal with such id. Kid id: {kidId}. Paht id: {pathId}. Goal id: {goalId}",
                    "Kid");
            }

            var goalDto = mapper.Map<GoalDto>(goal);

            return goalDto;
        }

        public async Task<Guid> CreateAsync(Guid kidId, Guid pathId, GoalDto goalDto)
        {
            var path = await unitOfWork.Paths.GetAsync(kidId, pathId);
            if (path == null)
            {
                throw new EntityNotFoundException(
                    $"Cannot find path to create goal. Kid id: {kidId}. Path id: {pathId}.",
                    "Path");
            }

            var goal = mapper.Map<Goal>(goalDto);
            var id = await unitOfWork.Goals.CreateAsync(pathId, goal);

            return id;
        }

        public async Task<Guid> UpdateAsync(Guid kidId, Guid pathId, GoalDto goalDto)
        {
            var path = await unitOfWork.Paths.GetAsync(kidId, pathId);
            if (path == null)
            {
                throw new EntityNotFoundException(
                    $"Cannot find path to update the goal. Kid id: {kidId}. Path id: {pathId}.",
                    "Path");
            }

            var goalToUpdate = await unitOfWork.Goals.GetAsync(pathId, goalDto.Id);
            if (goalToUpdate == null)
            {
                throw new EntityNotFoundException(
                    $"Cannot find goal to update. Id: {goalDto.Id}.",
                    "Goal");
            }

            var goal = mapper.Map<Goal>(goalDto);
            var id = await unitOfWork.Goals.UpdateAsync(pathId, goal);

            return id;
        }

        public async Task DeleteAsync(Guid kidId, Guid pathId, Guid goalId)
        {
            var path = await unitOfWork.Paths.GetAsync(kidId, pathId);

            if (path == null)
            {
                throw new EntityNotFoundException(
                    $"Cannot find path to delete the goal in. Kid id: {kidId}. Path id: {pathId}",
                    "Path");
            }

            var goal = await unitOfWork.Goals.GetAsync(pathId, goalId);

            if (goal == null)
            {
                throw new EntityNotFoundException(
                    $"Cannot find goal to delete. Path id: {pathId}. Goal id: {goalId}",
                    "Goal");
            }

            await unitOfWork.Goals.DeleteAsync(pathId, goalId);
        }
    }
}
