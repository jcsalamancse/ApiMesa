using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using MesaApi.Application.Common.Models;
using MesaApi.Domain.Entities;
using MesaApi.Domain.Enums;
using MesaApi.Domain.Interfaces;
using System.Security.Claims;

namespace MesaApi.Application.Features.Requests.Commands.AssignRequest;

public class AssignRequestCommandHandler : IRequestHandler<AssignRequestCommand, Result<AssignRequestResponse>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<AssignRequestCommandHandler> _logger;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AssignRequestCommandHandler(
        IUnitOfWork unitOfWork,
        ILogger<AssignRequestCommandHandler> logger,
        IHttpContextAccessor httpContextAccessor)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<Result<AssignRequestResponse>> Handle(AssignRequestCommand request, CancellationToken cancellationToken)
    {
        try
        {
            // Get current user ID from claims
            var userIdClaim = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var userName = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.Name)?.Value ?? "System";
            
            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out var userId))
            {
                return Result<AssignRequestResponse>.Failure("User not authenticated or invalid user ID");
            }

            // Get request
            var requestEntity = await _unitOfWork.Requests.GetByIdAsync(request.RequestId, cancellationToken);
            if (requestEntity == null)
            {
                return Result<AssignRequestResponse>.Failure($"Request with ID {request.RequestId} not found");
            }

            // Get assigned user
            var assignedUser = await _unitOfWork.Users.GetByIdAsync(request.AssignedToId, cancellationToken);
            if (assignedUser == null)
            {
                return Result<AssignRequestResponse>.Failure($"User with ID {request.AssignedToId} not found");
            }

            // Begin transaction
            await _unitOfWork.BeginTransactionAsync(cancellationToken);

            // Update request
            requestEntity.AssignedToId = request.AssignedToId;
            requestEntity.UpdatedAt = DateTime.UtcNow;
            requestEntity.UpdatedBy = userName;

            // If request is pending, change to in progress
            if (requestEntity.Status == RequestStatus.Pending)
            {
                requestEntity.Status = RequestStatus.InProgress;
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
                    Content = $"Request assigned to {assignedUser.Name}: {request.Comment}",
                    IsInternal = false,
                    CreatedBy = userName
                };

                var createdComment = await _unitOfWork.Comments.AddAsync(comment, cancellationToken);
                await _unitOfWork.SaveChangesAsync(cancellationToken);
                commentId = createdComment.Id;
            }

            // Add step for assignment
            var step = new RequestStep
            {
                RequestId = request.RequestId,
                StepName = $"Request assigned to {assignedUser.Name}",
                StepType = "Assignment",
                Order = (await _unitOfWork.RequestSteps.FindAsync(s => s.RequestId == request.RequestId, cancellationToken)).Count() + 1,
                Status = StepStatus.Completed,
                AssignedToId = request.AssignedToId,
                CompletedAt = DateTime.UtcNow,
                Notes = request.Comment,
                CreatedBy = userName
            };

            await _unitOfWork.RequestSteps.AddAsync(step, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            // Commit transaction
            await _unitOfWork.CommitTransactionAsync(cancellationToken);

            // Create response
            var response = new AssignRequestResponse(
                Id: requestEntity.Id,
                AssignedToId: request.AssignedToId,
                AssignedToName: assignedUser.Name,
                UpdatedAt: requestEntity.UpdatedAt ?? DateTime.UtcNow,
                UpdatedBy: userName,
                CommentId: commentId
            );

            _logger.LogInformation("Request {RequestId} assigned to user {AssignedToId} by user {UserId}", request.RequestId, request.AssignedToId, userId);
            
            return Result<AssignRequestResponse>.Success(response);
        }
        catch (Exception ex)
        {
            await _unitOfWork.RollbackTransactionAsync(cancellationToken);
            _logger.LogError(ex, "Error assigning request {RequestId} to user {AssignedToId}", request.RequestId, request.AssignedToId);
            return Result<AssignRequestResponse>.Failure("An error occurred while assigning the request");
        }
    }
}