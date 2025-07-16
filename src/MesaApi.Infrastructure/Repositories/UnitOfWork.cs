using Microsoft.EntityFrameworkCore.Storage;
using MesaApi.Domain.Entities;
using MesaApi.Domain.Interfaces;
using MesaApi.Infrastructure.Data;

namespace MesaApi.Infrastructure.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;
    private IDbContextTransaction? _transaction;

    public UnitOfWork(ApplicationDbContext context)
    {
        _context = context;
        Users = new UserRepository(_context);
        Requests = new RequestRepository(_context);
        Comments = new Repository<Comment>(_context);
        RequestSteps = new Repository<RequestStep>(_context);
        Roles = new Repository<Role>(_context);
        UserSessions = new Repository<UserSession>(_context);
        Attachments = new Repository<Attachment>(_context);
        Workflows = new Repository<Workflow>(_context);
        WorkflowSteps = new Repository<WorkflowStep>(_context);
    }

    public IUserRepository Users { get; }
    public IRequestRepository Requests { get; }
    public IRepository<Comment> Comments { get; }
    public IRepository<RequestStep> RequestSteps { get; }
    public IRepository<Role> Roles { get; }
    public IRepository<UserSession> UserSessions { get; }
    public IRepository<Attachment> Attachments { get; }
    public IRepository<Workflow> Workflows { get; }
    public IRepository<WorkflowStep> WorkflowSteps { get; }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        _transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
    }

    public async Task CommitTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (_transaction != null)
        {
            await _transaction.CommitAsync(cancellationToken);
            await _transaction.DisposeAsync();
            _transaction = null;
        }
    }

    public async Task RollbackTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (_transaction != null)
        {
            await _transaction.RollbackAsync(cancellationToken);
            await _transaction.DisposeAsync();
            _transaction = null;
        }
    }

    public void Dispose()
    {
        _transaction?.Dispose();
        _context.Dispose();
    }
}