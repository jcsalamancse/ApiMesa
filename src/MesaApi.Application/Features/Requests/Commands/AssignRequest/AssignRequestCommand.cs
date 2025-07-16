using MediatR;
using MesaApi.Application.Common.Models;

namespace MesaApi.Application.Features.Requests.Commands.AssignRequest;

public record AssignRequestCommand(
    int RequestId,
    int AssignedToId,
    string? Comment = null
) : IRequest<Result<AssignRequestResponse>>;

public record AssignRequestResponse(
    int Id,
    int AssignedToId,
    string AssignedToName,
    DateTime UpdatedAt,
    string UpdatedBy,
    int? CommentId = null
);