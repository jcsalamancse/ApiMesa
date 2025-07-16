using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MesaApi.Application.Common.Models;
using MesaApi.Infrastructure.Data;

namespace MesaApi.Application.Features.Roles.Queries.GetRoles;

public class GetRolesQueryHandler : IRequestHandler<GetRolesQuery, Result<List<RoleDto>>>
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<GetRolesQueryHandler> _logger;

    public GetRolesQueryHandler(
        ApplicationDbContext context,
        ILogger<GetRolesQueryHandler> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<Result<List<RoleDto>>> Handle(GetRolesQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var query = _context.Roles.AsQueryable();

            if (request.IsActive.HasValue)
            {
                query = query.Where(r => r.IsActive == request.IsActive.Value);
            }

            var roles = await query
                .OrderBy(r => r.Name)
                .Select(r => new RoleDto(
                    Id: r.Id,
                    Name: r.Name,
                    Description: r.Description,
                    IsActive: r.IsActive,
                    CreatedAt: r.CreatedAt
                ))
                .ToListAsync(cancellationToken);

            _logger.LogInformation("Retrieved {Count} roles", roles.Count);
            
            return Result<List<RoleDto>>.Success(roles);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving roles");
            return Result<List<RoleDto>>.Failure("An error occurred while retrieving roles");
        }
    }
}