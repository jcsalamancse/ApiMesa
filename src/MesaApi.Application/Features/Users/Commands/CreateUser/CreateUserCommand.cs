using MediatR;
using MesaApi.Application.Common.Models;

namespace MesaApi.Application.Features.Users.Commands.CreateUser;

public record CreateUserCommand(
    string Name,
    string Email,
    string Login,
    string Password,
    string? Phone = null,
    string? Department = null,
    string? Position = null
) : IRequest<Result<CreateUserResponse>>;

public record CreateUserResponse(
    int Id,
    string Name,
    string Email,
    string Login,
    string? Phone,
    string? Department,
    string? Position,
    DateTime CreatedAt
);