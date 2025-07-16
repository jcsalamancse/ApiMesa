using MediatR;
using MesaApi.Application.Common.Models;

namespace MesaApi.Application.Features.Users.Commands.DeleteUser;

public record DeleteUserCommand(int Id) : IRequest<Result>;