using MediatR;
using Microsoft.Extensions.Logging;
using MesaApi.Application.Common.Models;
using MesaApi.Domain.Interfaces;

namespace MesaApi.Application.Features.Roles.Queries.GetRoleById;

public class GetRoleByIdQueryHandler : IRequestHandler<GetRoleByIdQuery, Result<RoleDetailDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<GetRoleByIdQueryHandler> _logger;

    public GetRoleByIdQueryHandler(
        IUnitOfWork unitOfWork,
        ILogger<GetRoleByIdQueryHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Result<RoleDetailDto>> Handle(GetRoleByIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var role = await _unitOfWork.Roles.GetByIdAsync(request.Id, cancellationToken);

            if (role == null)
            {
                _logger.LogWarning("Role with ID {RoleId} not found", request.Id);
                return Result<RoleDetailDto>.Failure($"Role with ID {request.Id} not found");
            }

            // Get count of assigned users for this role
            var requestSteps = await _unitOfWork.RequestSteps.FindAsync(s => s.RoleId == role.Id, cancellationToken);
            var assignedUsersCount = requestSteps.Count();

            var roleDto = new RoleDetailDto(
                Id: role.Id,
                Name: role.Name,
                Description: role.Description,
                IsActive: role.IsActive,
                CreatedAt: role.CreatedAt,
                UpdatedAt: role.UpdatedAt,
                AssignedUsersCount: assignedUsersCount
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