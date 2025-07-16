using MediatR;
using MesaApi.Application.Common.Models;

namespace MesaApi.Application.Features.Requests.Queries.GetRequestAttachments;

public record GetRequestAttachmentsQuery(int RequestId) : IRequest<Result<List<AttachmentDto>>>;

public record AttachmentDto(
    int Id,
    int RequestId,
    string FileName,
    string FilePath,
    string ContentType,
    long FileSize,
    DateTime CreatedAt,
    string CreatedBy
);