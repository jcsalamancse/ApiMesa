using MediatR;
using MesaApi.Application.Common.Models;

namespace MesaApi.Application.Features.Users.Commands.UpdateUser;

public record UpdateUserCommand(
    int Id,
    string Name,
    string Email,
    string? Phone = null,
    string? Department = null,
    string? Position = null,
    bool? IsActive = null
) : IRequest<Result<UpdateUserResponse>>;

public record UpdateUserResponse(
    int Id,
    string Name,
    string Email,
    string Login,
    string? Phone,
    string? Department,
    string? Position,
    bool IsActive,
    DateTime UpdatedAt
);