using AutoMapper;
using GymManagementSystem.BusinessLogic.BusinessSpecifictions.BookingSpecifications;
using GymManagementSystem.BusinessLogic.BusinessSpecifictions.SessionSpecifications;
using GymManagementSystem.BusinessLogic.Contracts.Services;
using GymManagementSystem.BusinessLogic.DTOs.CategoryDTOs;
using GymManagementSystem.BusinessLogic.DTOs.SessionDTOs;
using GymManagementSystem.BusinessLogic.DTOs.TrainerDTOs;
using GymManagementSystem.BusinessLogic.Helpers.BusinessErrors;
using GymManagementSystem.BusinessLogic.Results;
using GymManagementSystem.DataAccess.Enums;
using GymManagementSystem.DataAccess.Models;
using GymManagementSystem.DataAccess.UoW.Contract;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementSystem.BusinessLogic.Services;

public sealed class SessionService : ISessionService
{
    /* Fields */
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<SessionService> _logger;

    /* Constructor */
    public SessionService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<SessionService> logger)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
    }

    /* Methods */
    public async Task<IEnumerable<AllSessionsDTO>> GetAllSessionsAsync(CancellationToken ct = default)
    {
        var sessionRepo = _unitOfWork.SessionRepository;
        var allSessionsWithTrainerAndCategorySpecification = new AllSessionsWithTrainerAndCategorySpecification();

        var allSessions = await sessionRepo.ListAsync(allSessionsWithTrainerAndCategorySpecification, ct:ct);

        if (allSessions is null || allSessions.Count == 0)
        {
            _logger.LogWarning("No sessions are retrieved when session's index page is hitted");
            return [];
        }

        var allSessionDTOs = _mapper.Map<IEnumerable<AllSessionsDTO>>(allSessions);

        var allSessionIds = new List<Guid>();
        allSessions.ToList().ForEach(s => allSessionIds.Add(s.Id));
        var numberOfBookingsPerSession = await sessionRepo.GetNumberOfBookingsPerSessions(allSessionIds,ct);

        if(numberOfBookingsPerSession is not null && numberOfBookingsPerSession.Count > 0)
            foreach(var allSessionDTO in  allSessionDTOs)
                allSessionDTO.NumberOfBookingsPerSession = numberOfBookingsPerSession[allSessionDTO.Id];

        _logger.LogInformation("Sessions: {@Sessions} are retrieved successfully", allSessionDTOs.Select(s => new
        {
            s.Id,
            s.CategoryName
        }));
        return allSessionDTOs;
    }

    public async Task<Result<IEnumerable<CategorySelectDTO>>> GetAllPossibleSessionCategoriesForDropDownListAsync(CancellationToken ct = default)
    {
        var categoryRepo = _unitOfWork.GetGenericRepository<Category>();

        var categories = await categoryRepo.GetAllAsync(ct: ct);

        if (categories is null || categories.Count == 0)
        {
            _logger.LogWarning("No category is retrieved to be displayed in drop down list when creating a new session");
            return Result<IEnumerable<CategorySelectDTO>>.Failure(SessionBusinessErrors.NewSessionCategoryDropDownListNotFound);
        }

        var categorySelectDTOs = _mapper.Map<IEnumerable<CategorySelectDTO>>(categories);

        _logger.LogInformation("Categories: {@Categories} are retrieved for drop down list when creating a new session", categorySelectDTOs);
        return Result<IEnumerable<CategorySelectDTO>>.Success(categorySelectDTOs);
    }

    public async Task<Result<IEnumerable<TrainerSelectDTO>>> GetAllPossibleSessionTrainersForDropDownListAsync(CancellationToken ct = default)
    {
        var trainerRepo = _unitOfWork.GetGenericRepository<Trainer>();

        var trainers = await trainerRepo.GetAllAsync(ct: ct);

        if (trainers is null || trainers.Count == 0)
        {
            _logger.LogWarning("No trainer is retrieved to be displayed in drop down list when creating a new session");
            return Result<IEnumerable<TrainerSelectDTO>>.Failure(SessionBusinessErrors.NewSessionTrainerDropDownListNotFound);
        }

        var trainerSelectDTOs = _mapper.Map<IEnumerable<TrainerSelectDTO>>(trainers);

        _logger.LogInformation("Trainers: {@Trainers} are retrieved for drop down list when creating a new session", trainerSelectDTOs);
        return Result<IEnumerable<TrainerSelectDTO>>.Success(trainerSelectDTOs);
    }

    public async Task<Result> AddSessionAsync(CreateSessionDTO createSessionDTO, CancellationToken ct = default)
    {
        if (createSessionDTO.StartDate > createSessionDTO.EndDate)
        {
            _logger.LogWarning("New session start date is later than end date");
            return Result.Failure(SessionBusinessErrors.NewSessionStartDateAfterEndDate);
        }  
        
        if (createSessionDTO.StartDate < DateTime.Now)
        {
            _logger.LogWarning("New session start date is in the past");
            return Result.Failure(SessionBusinessErrors.NewSessionStartDateAfterEndDate);
        }

        var categoryRepo = _unitOfWork.GetGenericRepository<Category>();
        var category = await categoryRepo.GetByIdAsync(createSessionDTO.CategoryId, ct: ct);

        if (category is null)
        {
            _logger.LogWarning("Selected category of Id ({@Id}) for the new session to be created is not found", createSessionDTO.CategoryId);
            return Result.Failure(SessionBusinessErrors.NewSessionCategoryNotFound);
        }

        if (Enum.TryParse(category.Name, true, out Speciality parsedCategoryName) && !Enum.IsDefined(parsedCategoryName))
        {
            _logger.LogWarning("Category of name ({@Name}) and ({@Id}) is invalid and doesn't match current specialities", category.Name, category.Id);
            return Result.Failure(SessionBusinessErrors.NewSessionInvalidCategory);
        }

        var trainerRepo = _unitOfWork.GetGenericRepository<Trainer>();
        var trainer = await trainerRepo.GetByIdAsync(createSessionDTO.TrainerId, ct: ct);

        if (trainer is null)
        {
            _logger.LogWarning("Selected trainer of Id ({@Id}) for the new session to be created is not found", createSessionDTO.TrainerId);
            return Result.Failure(SessionBusinessErrors.NewSessionTrainerNotFound);
        }

        if (parsedCategoryName != trainer.Speciality)
        {
            _logger.LogWarning("Selected category of name ({@Name}) is not matched with selected trainer speciality ({@Speciality}) for the new session to be created", parsedCategoryName, trainer.Speciality.ToString());
            return Result.Failure(SessionBusinessErrors.NewSessionCategoryAndTrainerSpecialityNotMatched);
        }

        var sessionRepo = _unitOfWork.SessionRepository;
        var createSessionTrainerIsNotAvailableSpecification = new CreateSessionTrainerIsNotAvailableSpecification(createSessionDTO.TrainerId,createSessionDTO.StartDate,createSessionDTO.EndDate);

        if (await sessionRepo.AnyAsync(createSessionTrainerIsNotAvailableSpecification, ct))
        {
            _logger.LogWarning("Selected trainer ({@Name}) is assigned to other sessions that overlap with this new session time", trainer.Name);
            return Result.Failure(SessionBusinessErrors.SessionTrainerUnavailable);
        }

        var session = _mapper.Map<Session>(createSessionDTO);

        sessionRepo.Add(session);

        var numOfRowsAffected = await _unitOfWork.SaveChangesAsync();

        if (numOfRowsAffected == 0)
        {
            _logger.LogError("New session is not created due to an internal DB error");
            return Result.Failure(SessionBusinessErrors.NewSessionNotCreated);
        }

        _logger.LogInformation("New session is created successfully with Id:{@Id}",session.Id);
        return Result.Success();
    }

    public async Task<Result<SessionDetailsDTO>> GetSessionDetailsAsync(Guid id, CancellationToken ct = default)
    {
        var sessionRepo = _unitOfWork.SessionRepository;
        var sessionWithCategoryAndTrainerSpecification = new SessionWithCategoryAndTrainerSpecification(id);

        var sessionDetails = await sessionRepo.FirstOrDefaultAsync(sessionWithCategoryAndTrainerSpecification,ct: ct);

        if(sessionDetails is null)
        {
            _logger.LogWarning("Session of Id ({@Id}) is not found", id);
            Result<SessionDetailsDTO>.Failure(SessionBusinessErrors.SessionWithDetailsNotFound);
        }
        
        var sessionDetailsDTO = _mapper.Map<SessionDetailsDTO>(sessionDetails);

        var bookingsPerSession = await sessionRepo.GetNumberOfBookingsPerSessions([id],ct: ct);
       
        if(bookingsPerSession is not null && bookingsPerSession.Count > 0)
            sessionDetailsDTO.NumberOfBookingsPerSession = bookingsPerSession[id];

        _logger.LogInformation("Session of Id ({@Id}) is retrieved successfully", id);
        return Result<SessionDetailsDTO>.Success(sessionDetailsDTO);
    }

    public async Task<Result<EditSessionDTO>> GetSessionToBeEditedAsync(Guid id, CancellationToken ct = default)
    {
        var sessionRepo = _unitOfWork.SessionRepository;
        var sessionWithTrainerSpecification = new SessionWithTrainerSpecification(id);

        var sessionToBeEdited = await sessionRepo.FirstOrDefaultAsync(sessionWithTrainerSpecification, ct: ct);

        if (sessionToBeEdited is null)
        {
            _logger.LogWarning("Session to be edited of Id ({@Id}) is not found", id);
            return Result<EditSessionDTO>.Failure(SessionBusinessErrors.SessionToBeEditedNotFound);
        }

        if (sessionToBeEdited.Status == "Completed" || sessionToBeEdited.Status == "Ongoing")
        {
            _logger.LogWarning("Session of Id ({@Id}) cannot be edited cause it's {@Status}", id, sessionToBeEdited.Status);
            return Result<EditSessionDTO>.Failure(SessionBusinessErrors.CannotEditOngoingOrCompletedSessions);
        }
        
        var sessionBookings = await sessionRepo.GetNumberOfBookingsPerSessions([id], ct: ct);

        if (sessionBookings is not null && sessionBookings.Count > 0 && sessionBookings[id] > 0)
        {
            _logger.LogWarning("Session of Id ({@Id}) cannot be edited cause it has active bookings)", id);
            return Result<EditSessionDTO>.Failure(SessionBusinessErrors.SessionToBeEditedHasActiveBookings);
        }
        
        var sessionToBeEditedDTO = _mapper.Map<EditSessionDTO>(sessionToBeEdited);

        _logger.LogInformation("Session to be edited of Id ({@Id}) is retrieved successfully", id);
        return Result<EditSessionDTO>.Success(sessionToBeEditedDTO);
    }

    public async Task<Result> EditSessionAsync(Guid id, EditSessionDTO sessionToBeEditedDTO, CancellationToken ct = default)
    {

        if (sessionToBeEditedDTO.StartDate > sessionToBeEditedDTO.EndDate)
        {
            _logger.LogWarning("Updated date(s) of session to be edited of Id ({@Id}) causes start date to be later than end date", id);
            return Result.Failure(SessionBusinessErrors.SessionToBeEditedStartDateAfterEndDate);
        }
        
        if (sessionToBeEditedDTO.StartDate < DateTime.Now)
        {
            _logger.LogWarning("Updated start date of session to be edited of Id ({@Id}) is in the past", id);
            return Result.Failure(SessionBusinessErrors.SessionToBeEditedStartDateInThePast);
        }
        
        var sessionRepo = _unitOfWork.SessionRepository;

        var sessionWithCategorySpecification = new SessionWithCategorySpecification(id);
        var sessionToBeEdited = await sessionRepo.FirstOrDefaultAsync(sessionWithCategorySpecification, ct: ct);

        if (sessionToBeEdited is null)
        {
            _logger.LogWarning("Session to be edited of Id ({@Id}) is not found",id);
            return Result.Failure(SessionBusinessErrors.SessionToBeEditedNotFound);
        }
        
        var trainerRepo = _unitOfWork.GetGenericRepository<Trainer>();
        var trainer = await trainerRepo.GetByIdAsync(sessionToBeEditedDTO.TrainerId, ct: ct);

        if (trainer is null)
        {
            _logger.LogWarning("Updated trainer of Id {@TrainerId} for session to be edited of Id ({@SessionId}) is not found", sessionToBeEditedDTO.TrainerId, id);
            return Result.Failure(SessionBusinessErrors.SessionToBeEditedTrainerNotFound);
        }
        
        var parsedCategoryName = Enum.Parse<Speciality>(sessionToBeEdited.Category.Name);

        if (parsedCategoryName != trainer.Speciality)
        {
            _logger.LogWarning("Category of name ({@Name}) is not matched with trainer speciality ({@Speciality}) for the session to be edited", parsedCategoryName, trainer.Speciality.ToString());
            return Result.Failure(SessionBusinessErrors.SessionToBeEditedCategoryAndTrainerSpecialityNotMatched);
        }   
        
        var editSessionTrainerIsNotAvailableSpecification = new EditSessionTrainerIsNotAvailableSpecification(id, sessionToBeEditedDTO.TrainerId, sessionToBeEditedDTO.StartDate, sessionToBeEditedDTO.EndDate);

        if (await sessionRepo.AnyAsync(editSessionTrainerIsNotAvailableSpecification, ct))
        {
            _logger.LogWarning("Updated trainer ({@Name}) is assigned to other sessions that overlap with time of session to be edited of Id ({@Id})", trainer.Name, id);
            return Result.Failure(SessionBusinessErrors.SessionTrainerUnavailable);
        }
        
        sessionRepo.Update(_mapper.Map(sessionToBeEditedDTO,sessionToBeEdited));

        var numOfAffectedRows = await _unitOfWork.SaveChangesAsync(ct);

        if (numOfAffectedRows == 0)
        {
            _logger.LogError("Session of Id ({@Id}) is not updated due to an internal DB error",id);
            return Result.Failure(SessionBusinessErrors.SessionNotEdited);
        }

        _logger.LogInformation("Session of Id ({@Id}) is updated successfully", id);
        return Result.Success();
    }

    public async Task<Result> CheckIfSessionToBeDeletedIsOngoingOrUpcomingAsync(Guid id, CancellationToken ct = default)
    {
        var sessionRepo = _unitOfWork.SessionRepository;
        var checkIfSessionIsOngoingOrUpcomingSpecification = new CheckIfSessionIsOngoingOrUpcomingSpecification(id);

        if (await sessionRepo.AnyAsync(checkIfSessionIsOngoingOrUpcomingSpecification, ct))
        {
            _logger.LogWarning("Session of Id ({@Id}) cannot be deleted because it's either ongoing or upcoming", id);
            return Result.Failure(SessionBusinessErrors.OngoingOrUpcomingSessionsCannotBeDeleted);
        }

        _logger.LogInformation("Session of Id ({@Id}) is allowed to be deleted cause it's an upcoming session", id);
        return Result.Success();
    }

    public async Task<Result> DeleteSessionAsync(Guid id, CancellationToken ct = default)
    {
        var sessionRepo = _unitOfWork.SessionRepository;
        var sessionToBeDeleted = await sessionRepo.GetByIdAsync(id);

        if (sessionToBeDeleted is null)
            return Result.Failure(SessionBusinessErrors.SessionNotDeleted);

        var bookingRepo = _unitOfWork.GetGenericRepository<Booking>();
        var bookingsForSessionExistSpecification = new BookingsForSessionExistSpecification(id);

        if (await bookingRepo.AnyAsync(bookingsForSessionExistSpecification, ct))
        {
            _logger.LogWarning("Session of Id ({@Id}) is not allowed to be deleted cause it has already active bookings", id);
            return Result.Failure(SessionBusinessErrors.CannotDeleteSessionWithBookings);
        }
        
        sessionRepo.SoftDelete(sessionToBeDeleted);

        var numberOfAffectedRows = await _unitOfWork.SaveChangesAsync();

        if (numberOfAffectedRows == 0)
        {
            _logger.LogError("Session of Id ({@Id}) is not deleted due to an internal DB error",id);
            return Result.Failure(SessionBusinessErrors.SessionNotDeleted);
        }

        _logger.LogInformation("Session of Id ({@Id}) is deleted successfully", id);
        return Result.Success();
    }
}
