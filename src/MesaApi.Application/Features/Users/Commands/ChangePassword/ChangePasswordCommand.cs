using MediatR;
using MesaApi.Application.Common.Models;

namespace MesaApi.Application.Features.Users.Commands.ChangePassword;

public record ChangePasswordCommand(
    int UserId,
    string CurrentPassword,
    string NewPassword
) : IRequest<Result>;