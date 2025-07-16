using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using MesaApi.Application.Common.Models;
using MesaApi.Domain.Interfaces;
using System.Security.Claims;

namespace MesaApi.Application.Features.Users.Commands.DeleteUser;

public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, Result>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<DeleteUserCommandHandler> _logger;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public DeleteUserCommandHandler(
        IUnitOfWork unitOfWork,
        ILogger<DeleteUserCommandHandler> logger,
        IHttpContextAccessor httpContextAccessor)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<Result> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        try
        {
            // Get current user ID from claims
            var currentUserIdClaim = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            
            if (string.IsNullOrEmpty(currentUserIdClaim) || !int.TryParse(currentUserIdClaim, out var currentUserId))
            {
                return Result.Failure("User not authenticated or invalid user ID");
            }

            // Prevent self-deletion
            if (currentUserId == request.Id)
            {
                return Result.Failure("Cannot delete your own account");
            }

            // Check if user exists
            var user = await _unitOfWork.Users.GetByIdAsync(request.Id, cancellationToken);
            if (user == null)
            {
                return Result.Failure($"User with ID {request.Id} not found");
            }

            // Check if user has associated requests
            var userRequests = await _unitOfWork.Requests.GetByRequesterIdAsync(request.Id, cancellationToken);
            if (userRequests.Any())
            {
                return Result.Failure("Cannot delete user with associated requests");
            }

            // Soft delete the user
            await _unitOfWork.Users.DeleteAsync(request.Id, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("User with ID {UserId} deleted successfully", request.Id);
            
            return Result.Success();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting user with ID: {UserId}", request.Id);
            return Result.Failure("An error occurred while deleting the user");
        }
    }
}