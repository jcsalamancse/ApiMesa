using MediatR;
using Microsoft.Extensions.Logging;
using MesaApi.Application.Common.Models;
using MesaApi.Domain.Interfaces;

namespace MesaApi.Application.Features.Users.Queries.GetUserById;

public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, Result<UserDetailDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<GetUserByIdQueryHandler> _logger;

    public GetUserByIdQueryHandler(
        IUnitOfWork unitOfWork,
        ILogger<GetUserByIdQueryHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Result<UserDetailDto>> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var user = await _unitOfWork.Users.GetByIdAsync(request.Id, cancellationToken);

            if (user == null)
            {
                _logger.LogWarning("User with ID {UserId} not found", request.Id);
                return Result<UserDetailDto>.Failure($"User with ID {request.Id} not found");
            }

            var userDto = new UserDetailDto(
                Id: user.Id,
                Name: user.Name,
                Email: user.Email,
                Login: user.Login,
                Phone: user.Phone,
                Department: user.Department,
                Position: user.Position,
                IsActive: user.IsActive,
                CreatedAt: user.CreatedAt,
                UpdatedAt: user.UpdatedAt
            );

            _logger.LogInformation("Retrieved user with ID: {UserId}", request.Id);
            
            return Result<UserDetailDto>.Success(userDto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving user with ID: {UserId}", request.Id);
            return Result<UserDetailDto>.Failure("An error occurred while retrieving the user");
        }
    }
}