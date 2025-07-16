using Microsoft.EntityFrameworkCore;
using MesaApi.Domain.Entities;
using MesaApi.Domain.Interfaces;
using MesaApi.Infrastructure.Data;

namespace MesaApi.Infrastructure.Repositories;

public class UserRepository : Repository<User>, IUserRepository
{
    public UserRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        return await _dbSet.FirstOrDefaultAsync(u => u.Email == email, cancellationToken);
    }

    public async Task<User?> GetByLoginAsync(string login, CancellationToken cancellationToken = default)
    {
        return await _dbSet.FirstOrDefaultAsync(u => u.Login == login, cancellationToken);
    }

    public async Task<bool> EmailExistsAsync(string email, CancellationToken cancellationToken = default)
    {
        return await _dbSet.AnyAsync(u => u.Email == email, cancellationToken);
    }

    public async Task<bool> LoginExistsAsync(string login, CancellationToken cancellationToken = default)
    {
        return await _dbSet.AnyAsync(u => u.Login == login, cancellationToken);
    }
}