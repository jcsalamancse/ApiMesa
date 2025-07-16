using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MesaApi.Application.Common.Models;
using MesaApi.Infrastructure.Data;

namespace MesaApi.Application.Features.Requests.Queries.GetRequestAttachments;

public class GetRequestAttachmentsQueryHandler : IRequestHandler<GetRequestAttachmentsQuery, Result<List<AttachmentDto>>>
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<GetRequestAttachmentsQueryHandler> _logger;

    public GetRequestAttachmentsQueryHandler(
        ApplicationDbContext context,
        ILogger<GetRequestAttachmentsQueryHandler> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<Result<List<AttachmentDto>>> Handle(GetRequestAttachmentsQuery request, CancellationToken cancellationToken)
    {
        try
        {
            // Check if request exists
            var requestExists = await _context.Requests.AnyAsync(r => r.Id == request.RequestId, cancellationToken);
            if (!requestExists)
            {
                return Result<List<AttachmentDto>>.Failure($"Request with ID {request.RequestId} not found");
            }

            // Get attachments
            var attachments = await _context.Attachments
                .Where(a => a.RequestId == request.RequestId)
                .OrderByDescending(a => a.CreatedAt)
                .Select(a => new AttachmentDto(
                    Id: a.Id,
                    RequestId: a.RequestId,
                    FileName: a.FileName,
                    FilePath: a.FilePath,
                    ContentType: a.ContentType,
                    FileSize: a.FileSize,
                    CreatedAt: a.CreatedAt,
                    CreatedBy: a.CreatedBy ?? "System"
                ))
                .ToListAsync(cancellationToken);

            _logger.LogInformation("Retrieved {Count} attachments for request {RequestId}", attachments.Count, request.RequestId);
            
            return Result<List<AttachmentDto>>.Success(attachments);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving attachments for request {RequestId}", request.RequestId);
            return Result<List<AttachmentDto>>.Failure("An error occurred while retrieving attachments");
        }
    }
}