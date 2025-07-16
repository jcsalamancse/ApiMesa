using MediatR;
using MesaApi.Application.Common.Models;

namespace MesaApi.Application.Features.Roles.Queries.GetRoles;

public record GetRolesQuery(
    bool? IsActive = null
) : IRequest<Result<List<RoleDto>>>;

public record RoleDto(
    int Id,
    string Name,
    string? Description,
    bool IsActive,
    DateTime CreatedAt
);