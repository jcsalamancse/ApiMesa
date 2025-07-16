using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using MesaApi.Application.Common.Models;
using MesaApi.Domain.Entities;
using MesaApi.Domain.Interfaces;
using System.Security.Claims;

namespace MesaApi.Application.Features.Comments.Commands.AddComment;

public class AddCommentCommandHandler : IRequestHandler<AddCommentCommand, Result<CommentResponse>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<AddCommentCommandHandler> _logger;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AddCommentCommandHandler(
        IUnitOfWork unitOfWork,
        ILogger<AddCommentCommandHandler> logger,
        IHttpContextAccessor httpContextAccessor)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<Result<CommentResponse>> Handle(AddCommentCommand request, CancellationToken cancellationToken)
    {
        try
        {
            // Get current user ID from claims
            var userIdClaim = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            
            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out var userId))
            {
                return Result<CommentResponse>.Failure("User not authenticated or invalid user ID");
            }

            // Check if request exists
            var requestEntity = await _unitOfWork.Requests.GetByIdAsync(request.RequestId, cancellationToken);
            if (requestEntity == null)
            {
                return Result<CommentResponse>.Failure($"Request with ID {request.RequestId} not found");
            }

            // Get user
            var user = await _unitOfWork.Users.GetByIdAsync(userId, cancellationToken);
            if (user == null)
            {
                return Result<CommentResponse>.Failure("User not found");
            }

            // Create comment
            var comment = new Comment
            {
                RequestId = request.RequestId,
                UserId = userId,
                Content = request.Content,
                IsInternal = request.IsInternal,
                CreatedBy = user.Name
            };

            // Save comment
            var createdComment = await _unitOfWork.Comments.AddAsync(comment, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            // Create response
            var response = new CommentResponse(
                Id: createdComment.Id,
                RequestId: createdComment.RequestId,
                Content: createdComment.Content,
                UserId: createdComment.UserId,
                UserName: user.Name,
                CreatedAt: createdComment.CreatedAt,
                IsInternal: createdComment.IsInternal
            );

            _logger.LogInformation("Comment added to request {RequestId} by user {UserId}", request.RequestId, userId);
            
            return Result<CommentResponse>.Success(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error adding comment to request {RequestId}", request.RequestId);
            return Result<CommentResponse>.Failure("An error occurred while adding the comment");
        }
    }
}