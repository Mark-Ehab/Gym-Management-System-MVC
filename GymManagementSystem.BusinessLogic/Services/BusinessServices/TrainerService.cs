using AutoMapper;
using AutoMapper.Execution;
using GymManagementSystem.BusinessLogic.BusinessSpecifictions.TrainerSpecifications;
using GymManagementSystem.BusinessLogic.Common;
using GymManagementSystem.BusinessLogic.Contracts.BusinessServices;
using GymManagementSystem.BusinessLogic.DTOs.MemberDTOs;
using GymManagementSystem.BusinessLogic.DTOs.TrainerDTOs;
using GymManagementSystem.BusinessLogic.Helpers.BusinessErrors;
using GymManagementSystem.DataAccess.Models;
using GymManagementSystem.DataAccess.Repositories.Contracts;
using GymManagementSystem.DataAccess.UoW.Contract;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementSystem.BusinessLogic.Services.BusinessServices;

public sealed class TrainerService : ITrainerService
{
    /* Fields */
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<TrainerService> _logger;

    /* Constructor */
    public TrainerService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<TrainerService> logger)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
    }

    /* Methods */
    public async Task<IEnumerable<AllTrainersDTO>> GetAllTrainersAsync(CancellationToken ct)
    {
        var trainerRepo = _unitOfWork.GetGenericRepository<Trainer>();

        var allTrainers = await trainerRepo.GetAllAsync(ct: ct);

        if (allTrainers is null || allTrainers.Count == 0)
        {
            _logger.LogWarning("No trainers are retrieved when trainer's index page is hitted");
            return [];
        }

        var allTrainersDTOs = _mapper.Map<IEnumerable<AllTrainersDTO>>(allTrainers);

        _logger.LogInformation("Trainers: {@Trainers} are retrieved successfully", allTrainersDTOs.Select(t => new
        {
            t.Id,
            t.Name
        }));
        return allTrainersDTOs;
    }

    public async Task<Result<TrainerDetailsDTO>> GetTrainerDetailsAsync(Guid id, CancellationToken ct)
    {
        var trainerRepo = _unitOfWork.GetGenericRepository<Trainer>();

        var trainer = await trainerRepo.GetByIdAsync(id, ct: ct);

        if (trainer is null)
        {
            _logger.LogWarning("Trainer of Id ({@Id}) is not found", id);
            return Result<TrainerDetailsDTO>.Failure(TrainerBusinessErrors.TrainerDetailsNotFound);
        }

        var trainerDetailsDTO = _mapper.Map<TrainerDetailsDTO>(trainer);

        _logger.LogInformation("Trainer {@Trainer} is retrieved successfully", new { Id = id, trainerDetailsDTO.Name });
        return Result<TrainerDetailsDTO>.Success(trainerDetailsDTO);
    }

    public async Task<Result> AddTrainerAsync(TrainerCreateDTO trainerToBeCreatedDTO, CancellationToken ct)
    {
        var trainerRepo = _unitOfWork.GetGenericRepository<Trainer>();

        /* Check if entered email already exists on the system */
        var newTrainerWithUniqueEmailSpecification = new NewTrainerWithUniqueEmailSpecification(trainerToBeCreatedDTO.Email);
        if (await trainerRepo.AnyAsync(newTrainerWithUniqueEmailSpecification,ct))
        {
            _logger.LogWarning("Email ({@Email}) of new trainer is not unique and already exists on the system", trainerToBeCreatedDTO.Email);
            return Result.Failure(TrainerBusinessErrors.TrainerEmailAlreadyExists);
        }
        
        /* Check if entered phone already exists on the system */
        var newTrainerWithUniquePhoneNumberSpecification = new NewTrainerWithUniquePhoneNumberSpecification(trainerToBeCreatedDTO.Phone);
        if (await trainerRepo.AnyAsync(newTrainerWithUniquePhoneNumberSpecification, ct))
        {
            _logger.LogWarning("Phone number ({@PhoneNumber}) of new trainer is not unique and already exists on the system", trainerToBeCreatedDTO.Phone);
            return Result.Failure(TrainerBusinessErrors.TrainerPhoneNumberAlreadyExists);
        }
        
        var trainerToBeAdded = _mapper.Map<Trainer>(trainerToBeCreatedDTO);

        trainerRepo.Add(trainerToBeAdded);

        var numOfRowsAffected = await _unitOfWork.SaveChangesAsync(ct);

        if (numOfRowsAffected == 0)
        {
            _logger.LogError("Trainer {@Name} is not created due to an internal DB error", trainerToBeCreatedDTO.Name);
            return Result.Failure(TrainerBusinessErrors.TrainerNotCreated);
        }

        _logger.LogInformation("Trainer {@Name} is created successfully with Id:{@Id}", trainerToBeCreatedDTO.Name, trainerToBeAdded.Id);
        return Result.Success();
    }

    public async Task<Result<TrainerToBeEditedDTO>> GetTrainerToBeEditedAsync(Guid id, CancellationToken ct)
    {
        var trainerRepo = _unitOfWork.GetGenericRepository<Trainer>();

        var trainerToBeEdited = await trainerRepo.GetByIdAsync(id, ct: ct);

        if (trainerToBeEdited is null)
        {
            _logger.LogWarning("Trainer to be edited of Id ({@Id}) is not found",id);
            return Result<TrainerToBeEditedDTO>.Failure(TrainerBusinessErrors.TrainerToBeEditedNotFound);
        }

        _logger.LogInformation("Trainer to be edited of Id ({@Id}) is retrieved successfully", id);
        return Result<TrainerToBeEditedDTO>.Success(_mapper.Map<TrainerToBeEditedDTO>(trainerToBeEdited));
    }

    public async Task<Result> EditTrainerAsync(Guid id, TrainerToBeEditedDTO trainerToBeEditedDTO, CancellationToken ct)
    {
        var trainerRepo = _unitOfWork.GetGenericRepository<Trainer>();

        /* Check if email already exists on the system */
        var editTrainerWithUniqueEmailSpecification = new EditTrainerWithUniqueEmailSpecification(id,trainerToBeEditedDTO.Email);
        if (await trainerRepo.AnyAsync(editTrainerWithUniqueEmailSpecification, ct))
        {
            _logger.LogWarning("Updated email ({@Email}) for trainer to be edited of Id ({@id}) is not unique and already exists on the system", trainerToBeEditedDTO.Email, id);
            return Result.Failure(TrainerBusinessErrors.TrainerEmailAlreadyExists);
        }
        
        /* Check if phone already exists on the system */
        var editTrainerWithUniquePhoneNumberSpecification = new EditTrainerWithUniquePhoneNumberSpecification(id,trainerToBeEditedDTO.Phone);
        if (await trainerRepo.AnyAsync(editTrainerWithUniquePhoneNumberSpecification, ct))
        {
            _logger.LogWarning("Updated phone number ({@PhoneNumber}) for trainer to be edited of Id ({@id}) is not unique and already exists on the system", trainerToBeEditedDTO.Phone, id);
            return Result.Failure(TrainerBusinessErrors.TrainerPhoneNumberAlreadyExists);
        }
        
        var trainerToBeEdited = await trainerRepo.GetByIdAsync(id, ct: ct);

        if (trainerToBeEdited is null)
        {
            _logger.LogWarning("Trainer to be edited of Id ({@Id}) is not found", id);
            return Result.Failure(TrainerBusinessErrors.TrainerToBeEditedNotFound);
        }
        
        _mapper.Map(trainerToBeEditedDTO, trainerToBeEdited);

        trainerRepo.Update(trainerToBeEdited);

        var numOfRowsAffected = await _unitOfWork.SaveChangesAsync(ct);

        if (numOfRowsAffected == 0)
        {
            _logger.LogError("Trainer of Id ({@Id}) is not updated due to an internal DB error", id);
            return Result.Failure(TrainerBusinessErrors.TrainerNotEdited);
        }

        _logger.LogInformation("(Trainer of Id ({@Id}) is updated successfully",id);
        return Result.Success();
    }

    public async Task<Result> DeleteTrainerAsync(Guid id, CancellationToken ct)
    {
        var trainerRepo = _unitOfWork.GetGenericRepository<Trainer>();

        var trainerToBeDeleted = await trainerRepo.GetByIdAsync(id, ct: ct);

        if (trainerToBeDeleted is null)
        {
            _logger.LogWarning("Trainer to be deleted of Id ({@Id}) is not found", id);
            return Result.Failure(TrainerBusinessErrors.TrainerToBeDeletedNotFound);
        }
        
        var sessionRepo = _unitOfWork.GetGenericRepository<Session>();
        var trainerHasScheduledSessionsSpecification = new TrainerHasScheduledSessionsSpecification(id);

        var trainerHasScheduledSessions = await sessionRepo.AnyAsync(trainerHasScheduledSessionsSpecification, ct);

        if (trainerHasScheduledSessions)
        {
            _logger.LogWarning("Trainer of Id ({@Id}) cannot be deleled as it has already scheduled sessions", id);
            return Result.Failure(TrainerBusinessErrors.TrainerWithScheduledSessionsCannotBeDeleted);
        }
        
        trainerRepo.SoftDelete(trainerToBeDeleted);

        var numOfRowsAffected = await _unitOfWork.SaveChangesAsync(ct);

        if (numOfRowsAffected == 0)
        {
            _logger.LogError("Trainer of Id ({@Id}) is not deleled due to internal DB error", id);
            return Result.Failure(TrainerBusinessErrors.TrainerNotDeleted);
        }

        _logger.LogInformation("Trainer of Id ({@Id}) is deleled successfully", id);
        return Result.Success();
    }
}
