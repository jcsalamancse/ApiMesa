using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using MesaApi.Application.Common.Models;
using MesaApi.Domain.Entities;
using MesaApi.Domain.Enums;
using MesaApi.Domain.Interfaces;
using System.Security.Claims;

namespace MesaApi.Application.Features.Requests.Commands.UpdateRequestStatus;

public class UpdateRequestStatusCommandHandler : IRequestHandler<UpdateRequestStatusCommand, Result<UpdateRequestStatusResponse>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<UpdateRequestStatusCommandHandler> _logger;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UpdateRequestStatusCommandHandler(
        IUnitOfWork unitOfWork,
        ILogger<UpdateRequestStatusCommandHandler> logger,
        IHttpContextAccessor httpContextAccessor)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<Result<UpdateRequestStatusResponse>> Handle(UpdateRequestStatusCommand request, CancellationToken cancellationToken)
    {
        try
        {
            // Get current user ID from claims
            var userIdClaim = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var userName = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.Name)?.Value ?? "System";
            
            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out var userId))
            {
                return Result<UpdateRequestStatusResponse>.Failure("User not authenticated or invalid user ID");
            }

            // Get request
            var requestEntity = await _unitOfWork.Requests.GetByIdAsync(request.RequestId, cancellationToken);
            if (requestEntity == null)
            {
                return Result<UpdateRequestStatusResponse>.Failure($"Request with ID {request.RequestId} not found");
            }

            // Begin transaction
            await _unitOfWork.BeginTransactionAsync(cancellationToken);

            // Update request status
            requestEntity.Status = request.NewStatus;
            requestEntity.UpdatedAt = DateTime.UtcNow;
            requestEntity.UpdatedBy = userName;

            // If status is completed, set completed date
            if (request.NewStatus == RequestStatus.Completed)
            {
                requestEntity.CompletedAt = DateTime.UtcNow;
            }

            await _unitOfWork.Requests.UpdateAsync(requestEntity, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            // Add comment if provided
            int? commentId = null;
            if (!string.IsNullOrWhiteSpace(request.Comment))
            {
                var comment = new Comment
                {
                    RequestId = request.RequestId,
                    UserId = userId,
                    Content = $"Status changed to {request.NewStatus}: {request.Comment}",
                    IsInternal = false,
                    CreatedBy = userName
                };

                var createdComment = await _unitOfWork.Comments.AddAsync(comment, cancellationToken);
                await _unitOfWork.SaveChangesAsync(cancellationToken);
                commentId = createdComment.Id;
            }

            // Add step for status change
            var step = new RequestStep
            {
                RequestId = request.RequestId,
                StepName = $"Status changed to {request.NewStatus}",
                StepType = "StatusChange",
                Order = (await _unitOfWork.RequestSteps.FindAsync(s => s.RequestId == request.RequestId, cancellationToken)).Count() + 1,
                Status = StepStatus.Completed,
                AssignedToId = userId,
                CompletedAt = DateTime.UtcNow,
                Notes = request.Comment,
                CreatedBy = userName
            };

            await _unitOfWork.RequestSteps.AddAsync(step, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            // Commit transaction
            await _unitOfWork.CommitTransactionAsync(cancellationToken);

            // Create response
            var response = new UpdateRequestStatusResponse(
                Id: requestEntity.Id,
                Status: requestEntity.Status.ToString(),
                UpdatedAt: requestEntity.UpdatedAt ?? DateTime.UtcNow,
                UpdatedBy: userName,
                CommentId: commentId
            );

            _logger.LogInformation("Request {RequestId} status updated to {Status} by user {UserId}", request.RequestId, request.NewStatus, userId);
            
            return Result<UpdateRequestStatusResponse>.Success(response);
        }
        catch (Exception ex)
        {
            await _unitOfWork.RollbackTransactionAsync(cancellationToken);
            _logger.LogError(ex, "Error updating status for request {RequestId}", request.RequestId);
            return Result<UpdateRequestStatusResponse>.Failure("An error occurred while updating the request status");
        }
    }
}