using Microsoft.EntityFrameworkCore;
using MesaApi.Domain.Entities;
using MesaApi.Domain.Enums;
using MesaApi.Domain.Interfaces;
using MesaApi.Infrastructure.Data;

namespace MesaApi.Infrastructure.Repositories;

public class RequestRepository : Repository<Request>, IRequestRepository
{
    public RequestRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Request>> GetByRequesterIdAsync(int requesterId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(r => r.Requester)
            .Include(r => r.AssignedTo)
            .Where(r => r.RequesterId == requesterId)
            .OrderByDescending(r => r.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Request>> GetByAssignedToIdAsync(int assignedToId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(r => r.Requester)
            .Include(r => r.AssignedTo)
            .Where(r => r.AssignedToId == assignedToId)
            .OrderByDescending(r => r.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Request>> GetByStatusAsync(RequestStatus status, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(r => r.Requester)
            .Include(r => r.AssignedTo)
            .Where(r => r.Status == status)
            .OrderByDescending(r => r.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<Request?> GetWithDetailsAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(r => r.Requester)
            .Include(r => r.AssignedTo)
            .Include(r => r.Steps)
                .ThenInclude(s => s.AssignedTo)
            .Include(r => r.Steps)
                .ThenInclude(s => s.Role)
            .Include(r => r.Comments)
                .ThenInclude(c => c.User)
            .Include(r => r.RequestData)
            .Include(r => r.Attachments)
            .FirstOrDefaultAsync(r => r.Id == id, cancellationToken);
    }

    public override async Task<IEnumerable<Request>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(r => r.Requester)
            .Include(r => r.AssignedTo)
            .OrderByDescending(r => r.CreatedAt)
            .ToListAsync(cancellationToken);
    }
}