using MediatR;
using MesaApi.Application.Common.Models;
using MesaApi.Domain.Enums;

namespace MesaApi.Application.Features.Requests.Commands.UpdateRequestStatus;

public record UpdateRequestStatusCommand(
    int RequestId,
    RequestStatus NewStatus,
    string? Comment = null
) : IRequest<Result<UpdateRequestStatusResponse>>;

public record UpdateRequestStatusResponse(
    int Id,
    string Status,
    DateTime UpdatedAt,
    string UpdatedBy,
    int? CommentId = null
);