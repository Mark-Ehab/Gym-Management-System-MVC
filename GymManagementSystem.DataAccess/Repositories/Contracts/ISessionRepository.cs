using GymManagementSystem.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementSystem.DataAccess.Repositories.Contracts;

public interface ISessionRepository : IGenericRepository<Session>
{
    Task<IDictionary<Guid, int>> GetNumberOfBookingsPerSessions(IReadOnlyList<Guid> sessionIds,CancellationToken ct);
}
