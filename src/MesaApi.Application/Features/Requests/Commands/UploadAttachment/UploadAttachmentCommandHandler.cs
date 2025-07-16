using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using MesaApi.Application.Common.Models;
using MesaApi.Domain.Entities;
using MesaApi.Domain.Interfaces;
using System.Security.Claims;
using System.IO;

namespace MesaApi.Application.Features.Requests.Commands.UploadAttachment;

public class UploadAttachmentCommandHandler : IRequestHandler<UploadAttachmentCommand, Result<UploadAttachmentResponse>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<UploadAttachmentCommandHandler> _logger;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IConfiguration _configuration;

    public UploadAttachmentCommandHandler(
        IUnitOfWork unitOfWork,
        ILogger<UploadAttachmentCommandHandler> logger,
        IHttpContextAccessor httpContextAccessor,
        IConfiguration configuration)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
        _httpContextAccessor = httpContextAccessor;
        _configuration = configuration;
    }

    public async Task<Result<UploadAttachmentResponse>> Handle(UploadAttachmentCommand request, CancellationToken cancellationToken)
    {
        try
        {
            // Get current user ID from claims
            var userIdClaim = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var userName = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.Name)?.Value ?? "System";
            
            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out var userId))
            {
                return Result<UploadAttachmentResponse>.Failure("User not authenticated or invalid user ID");
            }

            // Check if request exists
            var requestEntity = await _unitOfWork.Requests.GetByIdAsync(request.RequestId, cancellationToken);
            if (requestEntity == null)
            {
                return Result<UploadAttachmentResponse>.Failure($"Request with ID {request.RequestId} not found");
            }

            // Get file upload directory from configuration
            var uploadDirectory = _configuration["FileStorage:UploadDirectory"] ?? "uploads";
            var requestDirectory = Path.Combine(uploadDirectory, "requests", request.RequestId.ToString());
            
            // Create directory if it doesn't exist
            if (!Directory.Exists(requestDirectory))
            {
                Directory.CreateDirectory(requestDirectory);
            }

            // Generate unique filename
            var fileExtension = Path.GetExtension(request.File.FileName);
            var uniqueFileName = $"{Guid.NewGuid()}{fileExtension}";
            var filePath = Path.Combine(requestDirectory, uniqueFileName);

            // Save file to disk
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await request.File.CopyToAsync(stream, cancellationToken);
            }

            // Create attachment record
            var attachment = new Attachment
            {
                RequestId = request.RequestId,
                FileName = request.File.FileName,
                FilePath = filePath,
                ContentType = request.File.ContentType,
                FileSize = request.File.Length,
                CreatedBy = userName
            };

            // Save attachment record
            var createdAttachment = await _unitOfWork.Attachments.AddAsync(attachment, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            // Create response
            var response = new UploadAttachmentResponse(
                Id: createdAttachment.Id,
                RequestId: createdAttachment.RequestId,
                FileName: createdAttachment.FileName,
                FilePath: createdAttachment.FilePath,
                ContentType: createdAttachment.ContentType,
                FileSize: createdAttachment.FileSize,
                CreatedAt: createdAttachment.CreatedAt,
                CreatedBy: createdAttachment.CreatedBy ?? userName
            );

            _logger.LogInformation("Attachment uploaded for request {RequestId} by user {UserId}", request.RequestId, userId);
            
            return Result<UploadAttachmentResponse>.Success(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error uploading attachment for request {RequestId}", request.RequestId);
            return Result<UploadAttachmentResponse>.Failure("An error occurred while uploading the attachment");
        }
    }
}