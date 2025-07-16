using MediatR;
using MesaApi.Application.Common.Models;

namespace MesaApi.Application.Features.Users.Queries.GetUsers;

public record GetUsersQuery(
    int PageNumber = 1,
    int PageSize = 10,
    bool? IsActive = null,
    string? Department = null,
    string? SearchTerm = null
) : IRequest<Result<PagedResult<UserListItemDto>>>;

public record UserListItemDto(
    int Id,
    string Name,
    string Email,
    string Login,
    string? Department,
    string? Position,
    bool IsActive,
    DateTime CreatedAt
);