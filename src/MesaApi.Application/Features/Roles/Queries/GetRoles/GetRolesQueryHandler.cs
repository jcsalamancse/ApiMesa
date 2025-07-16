using MediatR;
using Microsoft.Extensions.Logging;
using MesaApi.Application.Common.Models;
using MesaApi.Domain.Interfaces;

namespace MesaApi.Application.Features.Roles.Queries.GetRoles;

public class GetRolesQueryHandler : IRequestHandler<GetRolesQuery, Result<List<RoleDto>>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<GetRolesQueryHandler> _logger;

    public GetRolesQueryHandler(
        IUnitOfWork unitOfWork,
        ILogger<GetRolesQueryHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Result<List<RoleDto>>> Handle(GetRolesQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var roles = await _unitOfWork.Roles.GetAllAsync(cancellationToken);

            var filteredRoles = roles.AsEnumerable();
            
            if (request.IsActive.HasValue)
            {
                filteredRoles = filteredRoles.Where(r => r.IsActive == request.IsActive.Value);
            }

            var roleDtos = filteredRoles
                .OrderBy(r => r.Name)
                .Select(r => new RoleDto(
                    Id: r.Id,
                    Name: r.Name,
                    Description: r.Description,
                    IsActive: r.IsActive,
                    CreatedAt: r.CreatedAt
                ))
                .ToList();

            _logger.LogInformation("Retrieved {Count} roles", roleDtos.Count);
            
            return Result<List<RoleDto>>.Success(roleDtos);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving roles");
            return Result<List<RoleDto>>.Failure("An error occurred while retrieving roles");
        }
    }
}