using System.Linq.Expressions;
using MesaApi.Domain.Entities;
using MesaApi.Domain.Enums;

namespace MesaApi.Domain.Interfaces;

public interface IRepository<T> where T : BaseEntity
{
    Task<T?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default);
    Task<T> AddAsync(T entity, CancellationToken cancellationToken = default);
    Task<T> UpdateAsync(T entity, CancellationToken cancellationToken = default);
    Task DeleteAsync(int id, CancellationToken cancellationToken = default);
    Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default);
    Task<int> CountAsync(Expression<Func<T, bool>>? predicate = null, CancellationToken cancellationToken = default);
}

public interface IUserRepository : IRepository<User>
{
    Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
    Task<User?> GetByLoginAsync(string login, CancellationToken cancellationToken = default);
    Task<bool> EmailExistsAsync(string email, CancellationToken cancellationToken = default);
    Task<bool> LoginExistsAsync(string login, CancellationToken cancellationToken = default);
}

public interface IRequestRepository : IRepository<Request>
{
    Task<IEnumerable<Request>> GetByRequesterIdAsync(int requesterId, CancellationToken cancellationToken = default);
    Task<IEnumerable<Request>> GetByAssignedToIdAsync(int assignedToId, CancellationToken cancellationToken = default);
    Task<IEnumerable<Request>> GetByStatusAsync(RequestStatus status, CancellationToken cancellationToken = default);
    Task<Request?> GetWithDetailsAsync(int id, CancellationToken cancellationToken = default);
}

public interface IUnitOfWork : IDisposable
{
    IUserRepository Users { get; }
    IRequestRepository Requests { get; }
    IRepository<Comment> Comments { get; }
    IRepository<RequestStep> RequestSteps { get; }
    IRepository<Role> Roles { get; }
    IRepository<UserSession> UserSessions { get; }
    IRepository<Attachment> Attachments { get; }
    IRepository<Workflow> Workflows { get; }
    IRepository<WorkflowStep> WorkflowSteps { get; }
    
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    Task BeginTransactionAsync(CancellationToken cancellationToken = default);
    Task CommitTransactionAsync(CancellationToken cancellationToken = default);
    Task RollbackTransactionAsync(CancellationToken cancellationToken = default);
}