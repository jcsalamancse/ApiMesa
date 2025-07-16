using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MesaApi.Application.Common.Models;
using MesaApi.Infrastructure.Data;

namespace MesaApi.Application.Features.Roles.Queries.GetRoleById;

public class GetRoleByIdQueryHandler : IRequestHandler<GetRoleByIdQuery, Result<RoleDetailDto>>
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<GetRoleByIdQueryHandler> _logger;

    public GetRoleByIdQueryHandler(
        ApplicationDbContext context,
        ILogger<GetRoleByIdQueryHandler> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<Result<RoleDetailDto>> Handle(GetRoleByIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var role = await _context.Roles
                .Where(r => r.Id == request.Id)
                .Select(r => new
                {
                    Role = r,
                    AssignedUsersCount = _context.RequestSteps.Count(s => s.RoleId == r.Id)
                })
                .FirstOrDefaultAsync(cancellationToken);

            if (role == null)
            {
                _logger.LogWarning("Role with ID {RoleId} not found", request.Id);
                return Result<RoleDetailDto>.Failure($"Role with ID {request.Id} not found");
            }

            var roleDto = new RoleDetailDto(
                Id: role.Role.Id,
                Name: role.Role.Name,
                Description: role.Role.Description,
                IsActive: role.Role.IsActive,
                CreatedAt: role.Role.CreatedAt,
                UpdatedAt: role.Role.UpdatedAt,
                AssignedUsersCount: role.AssignedUsersCount
            );

            _logger.LogInformation("Retrieved role with ID: {RoleId}", request.Id);
            
            return Result<RoleDetailDto>.Success(roleDto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving role with ID: {RoleId}", request.Id);
            return Result<RoleDetailDto>.Failure("An error occurred while retrieving the role");
        }
    }
}