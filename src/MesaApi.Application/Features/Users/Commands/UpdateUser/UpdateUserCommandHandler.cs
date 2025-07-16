using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using MesaApi.Application.Common.Models;
using MesaApi.Domain.Interfaces;
using System.Security.Claims;

namespace MesaApi.Application.Features.Users.Commands.UpdateUser;

public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, Result<UpdateUserResponse>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<UpdateUserCommandHandler> _logger;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UpdateUserCommandHandler(
        IUnitOfWork unitOfWork,
        ILogger<UpdateUserCommandHandler> logger,
        IHttpContextAccessor httpContextAccessor)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<Result<UpdateUserResponse>> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        try
        {
            // Get current user name for audit
            var currentUserName = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.Name)?.Value ?? "System";

            // Get user to update
            var user = await _unitOfWork.Users.GetByIdAsync(request.Id, cancellationToken);
            if (user == null)
            {
                return Result<UpdateUserResponse>.Failure($"User with ID {request.Id} not found");
            }

            // Check if email is being changed and if it already exists
            if (user.Email != request.Email && await _unitOfWork.Users.EmailExistsAsync(request.Email, cancellationToken))
            {
                return Result<UpdateUserResponse>.Failure("Email already exists");
            }

            // Update user properties
            user.Name = request.Name;
            user.Email = request.Email;
            user.Phone = request.Phone;
            user.Department = request.Department;
            user.Position = request.Position;
            user.IsActive = request.IsActive ?? user.IsActive;
            user.UpdatedAt = DateTime.UtcNow;
            user.UpdatedBy = currentUserName;

            // Save changes
            await _unitOfWork.Users.UpdateAsync(user, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            // Create response
            var response = new UpdateUserResponse(
                Id: user.Id,
                Name: user.Name,
                Email: user.Email,
                Login: user.Login,
                Phone: user.Phone,
                Department: user.Department,
                Position: user.Position,
                IsActive: user.IsActive,
                UpdatedAt: user.UpdatedAt ?? DateTime.UtcNow
            );

            _logger.LogInformation("User with ID {UserId} updated successfully", user.Id);
            
            return Result<UpdateUserResponse>.Success(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating user with ID: {UserId}", request.Id);
            return Result<UpdateUserResponse>.Failure("An error occurred while updating the user");
        }
    }
}