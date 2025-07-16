using MediatR;
using MesaApi.Application.Common.Models;

namespace MesaApi.Application.Features.Requests.Queries.GetRequestById;

public record GetRequestByIdQuery(int Id) : IRequest<Result<RequestDetailDto>>;

public record RequestDetailDto(
    int Id,
    string Description,
    string Status,
    string Priority,
    string? Category,
    string? SubCategory,
    int RequesterId,
    string RequesterName,
    int? AssignedToId,
    string? AssignedToName,
    DateTime? DueDate,
    DateTime CreatedAt,
    DateTime? CompletedAt,
    List<RequestStepDto> Steps,
    List<CommentDto> Comments,
    List<RequestDataItemDto> RequestData,
    List<AttachmentDto> Attachments
);

public record RequestStepDto(
    int Id,
    string StepName,
    string? StepType,
    int Order,
    string Status,
    int? AssignedToId,
    string? AssignedToName,
    int? RoleId,
    string? RoleName,
    DateTime? CompletedAt,
    string? Notes
);

public record CommentDto(
    int Id,
    string Content,
    int UserId,
    string UserName,
    DateTime CreatedAt,
    bool IsInternal
);

public record RequestDataItemDto(
    int Id,
    string Name,
    string Value,
    string? DataType
);

public record AttachmentDto(
    int Id,
    string FileName,
    string FilePath,
    string ContentType,
    long FileSize,
    DateTime CreatedAt
);