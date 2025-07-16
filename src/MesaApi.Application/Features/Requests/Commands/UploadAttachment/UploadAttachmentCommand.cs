using MediatR;
using Microsoft.AspNetCore.Http;
using MesaApi.Application.Common.Models;

namespace MesaApi.Application.Features.Requests.Commands.UploadAttachment;

public record UploadAttachmentCommand(
    int RequestId,
    IFormFile File,
    string? Description = null
) : IRequest<Result<UploadAttachmentResponse>>;

public record UploadAttachmentResponse(
    int Id,
    int RequestId,
    string FileName,
    string FilePath,
    string ContentType,
    long FileSize,
    DateTime CreatedAt,
    string CreatedBy
);