using MediatR;
using MesaApi.Application.Common.Models;

namespace MesaApi.Application.Features.Users.Queries.GetUserById;

public record GetUserByIdQuery(int Id) : IRequest<Result<UserDetailDto>>;

public record UserDetailDto(
    int Id,
    string Name,
    string Email,
    string Login,
    string? Phone,
    string? Department,
    string? Position,
    bool IsActive,
    DateTime CreatedAt,
    DateTime? UpdatedAt
);