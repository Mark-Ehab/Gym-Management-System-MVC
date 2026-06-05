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

    /* Constructor */
    public TrainerService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    /* Methods */
    public async Task<IEnumerable<AllTrainersDTO>> GetAllTrainersAsync(CancellationToken ct)
    {
        var trainerRepo = _unitOfWork.GetGenericRepository<Trainer>();

        var allTrainers = await trainerRepo.GetAllAsync(ct);

        if (allTrainers is null)
            return [];

        return allTrainers.Select(t => new AllTrainersDTO()
        {
            Id = t.Id,
            Name = t.Name,
            Email = t.Email,
            Phone = t.Phone,
            Speciality = t.Speciality
        });
    }

    public async Task<Result<TrainerDetailsDTO>> GetTrainerDetailsAsync(Guid id, CancellationToken ct)
    {
        var trainerRepo = _unitOfWork.GetGenericRepository<Trainer>();

        var trainer = await trainerRepo.GetByIdAsync(id, ct);

        if (trainer is null)
            return Result<TrainerDetailsDTO>.Failure(TrainerBusinessErrors.TrainerDetailsNotFound);

        var trainerDetailsDTO = new TrainerDetailsDTO()
        {
            Name = trainer.Name,
            Email = trainer.Email,
            Phone = trainer.Phone,
            DateOBirth = trainer.DateOfBirth,
            Speciality = trainer.Speciality,
            BuildingNumber = trainer.Address.BuildingNumber,
            Street = trainer.Address.Street,
            City = trainer.Address.City
        };

        return Result<TrainerDetailsDTO>.Success(trainerDetailsDTO);
    }

    public async Task<Result> AddTrainerAsync(TrainerCreateDTO trainerDTO, CancellationToken ct)
    {
        var trainerRepo = _unitOfWork.GetGenericRepository<Trainer>();

        /* Check if entered email already exists on the system */
        if (await trainerRepo.AnyAsync(t => t.Email == trainerDTO.Email,ct))
            return Result.Failure(TrainerBusinessErrors.TrainerEmailAlreadyExists);

        /* Check if entered phone already exists on the system */
        if (await trainerRepo.AnyAsync(t => t.Phone == trainerDTO.Phone,ct))
            return Result.Failure(TrainerBusinessErrors.TrainerPhoneNumberAlreadyExists);

        var trainerToBeAdded = new Trainer()
        {
            Name= trainerDTO.Name,
            Email= trainerDTO.Email,
            Phone= trainerDTO.Phone,
            DateOfBirth = trainerDTO.DateOfBirth,
            Gender = trainerDTO.Gender,
            Speciality= trainerDTO.Speciality,
            Address = new()
            {
                BuildingNumber = trainerDTO.BuildingNumber,
                Street = trainerDTO.Street,
                City = trainerDTO.City,
            }
        };

        trainerRepo.Add(trainerToBeAdded);

        var numOfRowsAffected = await _unitOfWork.SaveChangesAsync(ct);

        if (numOfRowsAffected == 0)
            return Result.Failure(TrainerBusinessErrors.TrainerNotCreated);

        return Result.Success();
    }

    public async Task<Result<TrainerToBeEditedDTO>> GetTrainerToBeEditedAsync(Guid id, CancellationToken ct)
    {
        var trainerRepo = _unitOfWork.GetGenericRepository<Trainer>();

        var trainerToBeEdited = await trainerRepo.GetByIdAsync(id, ct);

        if (trainerToBeEdited is null)
            return Result<TrainerToBeEditedDTO>.Failure(TrainerBusinessErrors.TrainerToBeEditedNotFound);

        var trainerToBeEditedDTO = new TrainerToBeEditedDTO()
        {
            Name = trainerToBeEdited.Name,
            Email = trainerToBeEdited.Email,
            Phone = trainerToBeEdited.Phone,
            Speciality = trainerToBeEdited.Speciality,
            BuildingNumber = trainerToBeEdited.Address.BuildingNumber,
            Street = trainerToBeEdited.Address.Street,
            City = trainerToBeEdited.Address.City,
        };

        return Result<TrainerToBeEditedDTO>.Success(trainerToBeEditedDTO);
    }

    public async Task<Result> EditTrainerAsync(Guid id, TrainerToBeEditedDTO trainerToBeEditedDTO, CancellationToken ct)
    {
        var trainerRepo = _unitOfWork.GetGenericRepository<Trainer>();

        /* Check if email already exists on the system */
        if (await trainerRepo.AnyAsync(t => t.Email == trainerToBeEditedDTO.Email && t.Id != id, ct))
            return Result.Failure(TrainerBusinessErrors.TrainerEmailAlreadyExists);

        /* Check if phone already exists on the system */
        if (await trainerRepo.AnyAsync(t => t.Phone == trainerToBeEditedDTO.Phone && t.Id != id, ct))
            return Result.Failure(TrainerBusinessErrors.TrainerPhoneNumberAlreadyExists);

        var trainerToBeEdited = await trainerRepo.GetByIdAsync(id,ct);

        if(trainerToBeEdited is null)
            return Result.Failure(TrainerBusinessErrors.TrainerToBeEditedNotFound);

        trainerToBeEdited.Email = trainerToBeEditedDTO.Email;
        trainerToBeEdited.Phone = trainerToBeEditedDTO.Phone;
        trainerToBeEdited.Speciality = trainerToBeEditedDTO.Speciality;
        trainerToBeEdited.Address.BuildingNumber = trainerToBeEditedDTO.BuildingNumber; 
        trainerToBeEdited.Address.Street = trainerToBeEditedDTO.Street; 
        trainerToBeEdited.Address.City = trainerToBeEditedDTO.City;

        trainerRepo.Update(trainerToBeEdited);

        var numOfRowsAffected = await _unitOfWork.SaveChangesAsync(ct);

        if (numOfRowsAffected == 0)
            return Result.Failure(TrainerBusinessErrors.TrainerNotEdited);

        return Result.Success();
    }

    public async Task<Result> DeleteTrainerAsync(Guid id, CancellationToken ct)
    {
        var trainerRepo = _unitOfWork.GetGenericRepository<Trainer>();

        var trainerToBeDeleted = await trainerRepo.GetByIdAsync(id, ct);

        if (trainerToBeDeleted is null)
            return Result.Failure(TrainerBusinessErrors.TrainerToBeDeletedNotFound);

        var sessionRepo = _unitOfWork.GetGenericRepository<Session>();

        var trainerHasScheduledSessions = await sessionRepo.AnyAsync(s => s.TrainerId == id, ct);

        if(trainerHasScheduledSessions)
            return Result.Failure(TrainerBusinessErrors.TrainerWithScheduledSessionsCannotBeDeleted);

        trainerRepo.SoftDelete(trainerToBeDeleted);

        var numOfRowsAffected = await _unitOfWork.SaveChangesAsync(ct);

        if (numOfRowsAffected == 0)
            return Result.Failure(TrainerBusinessErrors.TrainerNotDeleted);

        return Result.Success();
    }
}
