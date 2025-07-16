using MediatR;
using MesaApi.Application.Common.Models;
using MesaApi.Domain.Enums;

namespace MesaApi.Application.Features.Requests.Commands.CreateRequest;

public record CreateRequestCommand(
    string Description,
    RequestPriority Priority,
    string? Category,
    string? SubCategory,
    DateTime? DueDate,
    List<RequestDataDto>? RequestData = null
) : IRequest<Result<CreateRequestResponse>>;

public record RequestDataDto(
    string Name,
    string Value,
    string? DataType = null
);

public record CreateRequestResponse(
    int Id,
    string Description,
    string Status,
    string Priority,
    string? Category,
    string? SubCategory,
    int RequesterId,
    string RequesterName,
    DateTime? DueDate,
    DateTime CreatedAt,
    List<RequestDataDto>? RequestData
);