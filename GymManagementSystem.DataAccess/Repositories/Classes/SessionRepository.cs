using GymManagementSystem.DataAccess.Data.Contexts;
using GymManagementSystem.DataAccess.Models;
using GymManagementSystem.DataAccess.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;

namespace GymManagementSystem.DataAccess.Repositories.Classes;

public class SessionRepository : GenericRepository<Session>, ISessionRepository
{
    private readonly GymDbContext _gymDbContext;

    public SessionRepository(GymDbContext gymDbContext):
        base(gymDbContext)
    {
       _gymDbContext = gymDbContext;
    }
    public async Task<IDictionary<Guid, int>> GetNumberOfBookingsPerSessions(IReadOnlyList<Guid> sessionIds, CancellationToken ct)
        => await _gymDbContext.Bookings
            .Where(b => sessionIds.Contains(b.SessionId))
            .GroupBy(b => b.SessionId).ToDictionaryAsync(g => g.Key,g => g.Count(),ct);
}
