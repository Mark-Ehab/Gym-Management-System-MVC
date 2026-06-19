using GymManagementSystem.BusinessLogic.Common;
using GymManagementSystem.BusinessLogic.DTOs.CategoryDTOs;
using GymManagementSystem.BusinessLogic.DTOs.SessionDTOs;
using GymManagementSystem.BusinessLogic.DTOs.TrainerDTOs;
using GymManagementSystem.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementSystem.BusinessLogic.Contracts.BusinessServices;

public interface ISessionService
{
    Task<IEnumerable<AllSessionsDTO>> GetAllSessionsAsync(CancellationToken ct = default);
    Task<Result<IEnumerable<CategorySelectDTO>>> GetAllPossibleSessionCategoriesForDropDownListAsync(CancellationToken ct = default);
    Task<Result<IEnumerable<TrainerSelectDTO>>> GetAllPossibleSessionTrainersForDropDownListAsync(CancellationToken ct = default);
    Task<Result> AddSessionAsync(CreateSessionDTO createSessionDTO, CancellationToken ct = default);
    Task<Result<SessionDetailsDTO>> GetSessionDetailsAsync(Guid id, CancellationToken ct = default);
    Task<Result<EditSessionDTO>> GetSessionToBeEditedAsync(Guid id, CancellationToken ct = default);
    Task<Result> EditSessionAsync(Guid id, EditSessionDTO sessionToBeEditedDTO, CancellationToken ct = default);
    Task<Result> CheckIfSessionToBeDeletedIsOngoingOrUpcomingAsync(Guid id, CancellationToken ct = default);
    Task<Result> DeleteSessionAsync(Guid id, CancellationToken ct = default);
}
