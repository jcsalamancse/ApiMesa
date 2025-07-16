using MediatR;
using MesaApi.Application.Common.Models;

namespace MesaApi.Application.Features.Auth.Commands.Login;

public record LoginCommand(
    string Login,
    string Password
) : IRequest<Result<LoginResponse>>;

public record LoginResponse(
    string Token,
    string RefreshToken,
    DateTime ExpiresAt,
    UserInfo User
);

public record UserInfo(
    int Id,
    string Name,
    string Email,
    string Login,
    string? Department,
    string? Position
);