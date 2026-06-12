using AutoMapper;
using GymManagementSystem.BusinessLogic.BusinessSpecifictions.TrainerSpecifications;
using GymManagementSystem.BusinessLogic.Contracts.Services;
using GymManagementSystem.BusinessLogic.DTOs.TrainerDTOs;
using GymManagementSystem.BusinessLogic.Helpers.BusinessErrors;
using GymManagementSystem.BusinessLogic.Results;
using GymManagementSystem.DataAccess.Models;
using GymManagementSystem.DataAccess.Repositories.Contracts;
using GymManagementSystem.DataAccess.UoW.Contract;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementSystem.BusinessLogic.Services;

public sealed class TrainerService : ITrainerService
{
    /* Fields */
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    /* Constructor */
    public TrainerService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    /* Methods */
    public async Task<IEnumerable<AllTrainersDTO>> GetAllTrainersAsync(CancellationToken ct)
    {
        var trainerRepo = _unitOfWork.GetGenericRepository<Trainer>();

        var allTrainers = await trainerRepo.GetAllAsync(ct: ct);

        if (allTrainers is null || allTrainers.Count == 0)
            return [];

        return _mapper.Map<IEnumerable<AllTrainersDTO>>(allTrainers);
    }

    public async Task<Result<TrainerDetailsDTO>> GetTrainerDetailsAsync(Guid id, CancellationToken ct)
    {
        var trainerRepo = _unitOfWork.GetGenericRepository<Trainer>();

        var trainer = await trainerRepo.GetByIdAsync(id, ct: ct);

        if (trainer is null)
            return Result<TrainerDetailsDTO>.Failure(TrainerBusinessErrors.TrainerDetailsNotFound);

        return Result<TrainerDetailsDTO>.Success(_mapper.Map<TrainerDetailsDTO>(trainer));
    }

    public async Task<Result> AddTrainerAsync(TrainerCreateDTO trainerToBeCreatedDTO, CancellationToken ct)
    {
        var trainerRepo = _unitOfWork.GetGenericRepository<Trainer>();
        var newTrainerWithUniqueEmailSpecification = new NewTrainerWithUniqueEmailSpecification(trainerToBeCreatedDTO.Email);
        var newTrainerWithUniquePhoneNumberSpecification = new NewTrainerWithUniquePhoneNumberSpecification(trainerToBeCreatedDTO.Phone);

        /* Check if entered email already exists on the system */
        if (await trainerRepo.AnyAsync(newTrainerWithUniqueEmailSpecification,ct))
            return Result.Failure(TrainerBusinessErrors.TrainerEmailAlreadyExists);

        /* Check if entered phone already exists on the system */
        if (await trainerRepo.AnyAsync(newTrainerWithUniquePhoneNumberSpecification, ct))
            return Result.Failure(TrainerBusinessErrors.TrainerPhoneNumberAlreadyExists);

        var trainerToBeAdded = _mapper.Map<Trainer>(trainerToBeCreatedDTO);

        trainerRepo.Add(trainerToBeAdded);

        var numOfRowsAffected = await _unitOfWork.SaveChangesAsync(ct);

        if (numOfRowsAffected == 0)
            return Result.Failure(TrainerBusinessErrors.TrainerNotCreated);

        return Result.Success();
    }

    public async Task<Result<TrainerToBeEditedDTO>> GetTrainerToBeEditedAsync(Guid id, CancellationToken ct)
    {
        var trainerRepo = _unitOfWork.GetGenericRepository<Trainer>();

        var trainerToBeEdited = await trainerRepo.GetByIdAsync(id, ct: ct);

        if (trainerToBeEdited is null)
            return Result<TrainerToBeEditedDTO>.Failure(TrainerBusinessErrors.TrainerToBeEditedNotFound);

        return Result<TrainerToBeEditedDTO>.Success(_mapper.Map<TrainerToBeEditedDTO>(trainerToBeEdited));
    }

    public async Task<Result> EditTrainerAsync(Guid id, TrainerToBeEditedDTO trainerToBeEditedDTO, CancellationToken ct)
    {
        var trainerRepo = _unitOfWork.GetGenericRepository<Trainer>();
        var editTrainerWithUniqueEmailSpecification = new EditTrainerWithUniqueEmailSpecification(id,trainerToBeEditedDTO.Email);
        var editTrainerWithUniquePhoneNumberSpecification = new EditTrainerWithUniquePhoneNumberSpecification(id,trainerToBeEditedDTO.Phone);

        /* Check if email already exists on the system */
        if (await trainerRepo.AnyAsync(editTrainerWithUniqueEmailSpecification, ct))
            return Result.Failure(TrainerBusinessErrors.TrainerEmailAlreadyExists);

        /* Check if phone already exists on the system */
        if (await trainerRepo.AnyAsync(editTrainerWithUniquePhoneNumberSpecification, ct))
            return Result.Failure(TrainerBusinessErrors.TrainerPhoneNumberAlreadyExists);

        var trainerToBeEdited = await trainerRepo.GetByIdAsync(id, ct: ct);

        if (trainerToBeEdited is null)
            return Result.Failure(TrainerBusinessErrors.TrainerToBeEditedNotFound);

        _mapper.Map(trainerToBeEditedDTO, trainerToBeEdited);

        trainerRepo.Update(trainerToBeEdited);

        var numOfRowsAffected = await _unitOfWork.SaveChangesAsync(ct);

        if (numOfRowsAffected == 0)
            return Result.Failure(TrainerBusinessErrors.TrainerNotEdited);

        return Result.Success();
    }

    public async Task<Result> DeleteTrainerAsync(Guid id, CancellationToken ct)
    {
        var trainerRepo = _unitOfWork.GetGenericRepository<Trainer>();

        var trainerToBeDeleted = await trainerRepo.GetByIdAsync(id, ct: ct);

        if (trainerToBeDeleted is null)
            return Result.Failure(TrainerBusinessErrors.TrainerToBeDeletedNotFound);

        var sessionRepo = _unitOfWork.GetGenericRepository<Session>();
        var trainerHasScheduledSessionsSpecification = new TrainerHasScheduledSessionsSpecification(id);

        var trainerHasScheduledSessions = await sessionRepo.AnyAsync(trainerHasScheduledSessionsSpecification, ct);

        if (trainerHasScheduledSessions)
            return Result.Failure(TrainerBusinessErrors.TrainerWithScheduledSessionsCannotBeDeleted);

        trainerRepo.SoftDelete(trainerToBeDeleted);

        var numOfRowsAffected = await _unitOfWork.SaveChangesAsync(ct);

        if (numOfRowsAffected == 0)
            return Result.Failure(TrainerBusinessErrors.TrainerNotDeleted);

        return Result.Success();
    }
}
