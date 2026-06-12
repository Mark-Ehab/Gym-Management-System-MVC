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
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementSystem.BusinessLogic.Services;

public sealed class SessionService : ISessionService
{
    /* Fields */
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    /* Constructor */
    public SessionService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    /* Methods */
    public async Task<IEnumerable<AllSessionsDTO>> GetAllSessionsAsync(CancellationToken ct = default)
    {
        var sessionRepo = _unitOfWork.SessionRepository;
        var allSessionsWithTrainerAndCategorySpecification = new AllSessionsWithTrainerAndCategorySpecification();

        var allSessions = await sessionRepo.ListAsync(allSessionsWithTrainerAndCategorySpecification, ct:ct);

        if (allSessions is null || allSessions.Count == 0)
            return [];

        var allSessionDTOs = _mapper.Map<IEnumerable<AllSessionsDTO>>(allSessions);

        var allSessionIds = new List<Guid>();
        allSessions.ToList().ForEach(s => allSessionIds.Add(s.Id));
        var numberOfBookingsPerSession = await sessionRepo.GetNumberOfBookingsPerSessions(allSessionIds,ct);

        if(numberOfBookingsPerSession is not null && numberOfBookingsPerSession.Count > 0)
            foreach(var allSessionDTO in  allSessionDTOs)
                allSessionDTO.NumberOfBookingsPerSession = numberOfBookingsPerSession[allSessionDTO.Id];
            
        return allSessionDTOs;
    }

    public async Task<Result<IEnumerable<CategorySelectDTO>>> GetAllPossibleSessionCategoriesForDropDownListAsync(CancellationToken ct = default)
    {
        var categoryRepo = _unitOfWork.GetGenericRepository<Category>();

        var categories = await categoryRepo.GetAllAsync(ct: ct);

        if (categories is null || categories.Count == 0)
            return Result<IEnumerable<CategorySelectDTO>>.Failure(SessionBusinessErrors.NewSessionCategoryDropDownListNotFound);

        return Result<IEnumerable<CategorySelectDTO>>.Success(_mapper.Map<IEnumerable<CategorySelectDTO>>(categories));
    }

    public async Task<Result<IEnumerable<TrainerSelectDTO>>> GetAllPossibleSessionTrainersForDropDownListAsync(CancellationToken ct = default)
    {
        var trainerRepo = _unitOfWork.GetGenericRepository<Trainer>();

        var trainers = await trainerRepo.GetAllAsync(ct: ct);

        if (trainers is null || trainers.Count == 0)
            return Result<IEnumerable<TrainerSelectDTO>>.Failure(SessionBusinessErrors.NewSessionTrainerDropDownListNotFound);

        return Result<IEnumerable<TrainerSelectDTO>>.Success(_mapper.Map<IEnumerable<TrainerSelectDTO>>(trainers));
    }

    public async Task<Result> AddSessionAsync(CreateSessionDTO createSessionDTO, CancellationToken ct = default)
    {
        if (createSessionDTO.StartDate > createSessionDTO.EndDate)
            return Result.Failure(SessionBusinessErrors.NewSessionStartDateAfterEndDate);
    
        if (createSessionDTO.StartDate < DateTime.Now)
            return Result.Failure(SessionBusinessErrors.NewSessionStartDateAfterEndDate);

        var categoryRepo = _unitOfWork.GetGenericRepository<Category>();
        var category = await categoryRepo.GetByIdAsync(createSessionDTO.CategoryId, ct: ct);

        if (category is null)
            return Result.Failure(SessionBusinessErrors.NewSessionCategoryNotFound);

        if (Enum.TryParse(category.Name, true, out Speciality parsedCategoryName) && !Enum.IsDefined(parsedCategoryName))
            return Result.Failure(SessionBusinessErrors.NewSessionInvalidCategory);

        var trainerRepo = _unitOfWork.GetGenericRepository<Trainer>();
        var trainer = await trainerRepo.GetByIdAsync(createSessionDTO.TrainerId, ct: ct);

        if (trainer is null)
            return Result.Failure(SessionBusinessErrors.NewSessionTrainerNotFound);

        if (parsedCategoryName != trainer.Speciality)
            return Result.Failure(SessionBusinessErrors.NewSessionCategoryAndTrainerSpecialityNotMatched);

        var sessionRepo = _unitOfWork.SessionRepository;
        var createSessionTrainerIsNotAvailableSpecification = new CreateSessionTrainerIsNotAvailableSpecification(createSessionDTO.TrainerId,createSessionDTO.StartDate,createSessionDTO.EndDate);

        if (await sessionRepo.AnyAsync(createSessionTrainerIsNotAvailableSpecification, ct))
            return Result.Failure(SessionBusinessErrors.SessionTrainerUnavailable);

        sessionRepo.Add(_mapper.Map<Session>(createSessionDTO));

        var numOfRowsAffected = await _unitOfWork.SaveChangesAsync();

        if (numOfRowsAffected == 0)
            return Result.Failure(SessionBusinessErrors.NewSessionNotCreated);

        return Result.Success();
    }

    public async Task<Result<SessionDetailsDTO>> GetSessionDetailsAsync(Guid id, CancellationToken ct = default)
    {
        var sessionRepo = _unitOfWork.SessionRepository;
        var sessionWithCategoryAndTrainerSpecification = new SessionWithCategoryAndTrainerSpecification(id);

        var sessionDetails = await sessionRepo.FirstOrDefaultAsync(sessionWithCategoryAndTrainerSpecification,ct: ct);

        if(sessionDetails is null)
            Result<SessionDetailsDTO>.Failure(SessionBusinessErrors.SessionWithDetailsNotFound);

        var sessionDetailsDTO = _mapper.Map<SessionDetailsDTO>(sessionDetails);

        var bookingsPerSession = await sessionRepo.GetNumberOfBookingsPerSessions([id],ct: ct);
       
        if(bookingsPerSession is not null && bookingsPerSession.Count > 0)
            sessionDetailsDTO.NumberOfBookingsPerSession = bookingsPerSession[id];

        return Result<SessionDetailsDTO>.Success(sessionDetailsDTO);
    }

    public async Task<Result<EditSessionDTO>> GetSessionToBeEditedAsync(Guid id, CancellationToken ct = default)
    {
        var sessionRepo = _unitOfWork.SessionRepository;
        var sessionWithTrainerSpecification = new SessionWithTrainerSpecification(id);

        var sessionToBeEdited = await sessionRepo.FirstOrDefaultAsync(sessionWithTrainerSpecification, ct: ct);

        if (sessionToBeEdited is null)
            return Result<EditSessionDTO>.Failure(SessionBusinessErrors.SessionToBeEditedNotFound);

        if (sessionToBeEdited.Status == "Completed" || sessionToBeEdited.Status == "Ongoing")
            return Result<EditSessionDTO>.Failure(SessionBusinessErrors.CannotEditOngoingOrCompletedSessions);

        var sessionBookings = await sessionRepo.GetNumberOfBookingsPerSessions([id], ct: ct);

        if (sessionBookings is not null && sessionBookings.Count > 0 && sessionBookings[id] > 0)
            return Result<EditSessionDTO>.Failure(SessionBusinessErrors.SessionToBeEditedHasActiveBookings);

        var sessionToBeEditedDTO = _mapper.Map<EditSessionDTO>(sessionToBeEdited);

        return Result<EditSessionDTO>.Success(sessionToBeEditedDTO);
    }

    public async Task<Result> EditSessionAsync(Guid id, EditSessionDTO sessionToBeEditedDTO, CancellationToken ct = default)
    {

        if (sessionToBeEditedDTO.StartDate > sessionToBeEditedDTO.EndDate)
            return Result.Failure(SessionBusinessErrors.SessionToBeEditedStartDateAfterEndDate);

        if (sessionToBeEditedDTO.StartDate < DateTime.Now)
            return Result.Failure(SessionBusinessErrors.SessionToBeEditedStartDateInThePast);

        var sessionRepo = _unitOfWork.SessionRepository;

        var sessionWithCategorySpecification = new SessionWithCategorySpecification(id);
        var sessionToBeEdited = await sessionRepo.FirstOrDefaultAsync(sessionWithCategorySpecification, ct: ct);

        if (sessionToBeEdited is null)
            return Result.Failure(SessionBusinessErrors.SessionToBeEditedNotFound);

        var trainerRepo = _unitOfWork.GetGenericRepository<Trainer>();
        var trainer = await trainerRepo.GetByIdAsync(sessionToBeEditedDTO.TrainerId, ct: ct);

        if (trainer is null)
            return Result.Failure(SessionBusinessErrors.SessionToBeEditedTrainerNotFound);

        var parsedCategoryName = Enum.Parse<Speciality>(sessionToBeEdited.Category.Name);

        if (parsedCategoryName != trainer.Speciality)
            return Result.Failure(SessionBusinessErrors.SessionToBeEditedCategoryAndTrainerSpecialityNotMatched);

        var editSessionTrainerIsNotAvailableSpecification = new EditSessionTrainerIsNotAvailableSpecification(id, sessionToBeEditedDTO.TrainerId, sessionToBeEditedDTO.StartDate, sessionToBeEditedDTO.EndDate);

        if (await sessionRepo.AnyAsync(editSessionTrainerIsNotAvailableSpecification, ct))
            return Result.Failure(SessionBusinessErrors.SessionTrainerUnavailable);

        sessionRepo.Update(_mapper.Map(sessionToBeEditedDTO,sessionToBeEdited));

        var numOfAffectedRows = await _unitOfWork.SaveChangesAsync(ct);

        if (numOfAffectedRows == 0)
            return Result.Failure(SessionBusinessErrors.SessionNotEdited);

        return Result.Success();
    }

    public async Task<Result> CheckIfSessionToBeDeletedIsOngoingOrUpcomingAsync(Guid id, CancellationToken ct = default)
    {
        var sessionRepo = _unitOfWork.SessionRepository;
        var checkIfSessionIsOngoingOrUpcomingSpecification = new CheckIfSessionIsOngoingOrUpcomingSpecification(id);

        if (await sessionRepo.AnyAsync(checkIfSessionIsOngoingOrUpcomingSpecification, ct))
            return Result.Failure(SessionBusinessErrors.OngoingOrUpcomingSessionsCannotBeDeleted);

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
            return Result.Failure(SessionBusinessErrors.CannotDeleteSessionWithBookings);

        sessionRepo.SoftDelete(sessionToBeDeleted);

        var numberOfAffectedRows = await _unitOfWork.SaveChangesAsync();

        if (numberOfAffectedRows == 0)
            return Result.Failure(SessionBusinessErrors.SessionNotDeleted);

        return Result.Success();
    }
}
