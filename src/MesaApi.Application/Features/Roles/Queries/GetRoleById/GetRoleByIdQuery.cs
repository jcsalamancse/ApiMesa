using MediatR;
using MesaApi.Application.Common.Models;

namespace MesaApi.Application.Features.Roles.Queries.GetRoleById;

public record GetRoleByIdQuery(int Id) : IRequest<Result<RoleDetailDto>>;

public record RoleDetailDto(
    int Id,
    string Name,
    string? Description,
    bool IsActive,
    DateTime CreatedAt,
    DateTime? UpdatedAt,
    int AssignedUsersCount
);