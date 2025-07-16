using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using MesaApi.Application.Common.Models;
using MesaApi.Domain.Entities;
using MesaApi.Domain.Interfaces;
using BCrypt.Net;

namespace MesaApi.Application.Features.Users.Commands.CreateUser;

public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, Result<CreateUserResponse>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<CreateUserCommandHandler> _logger;

    public CreateUserCommandHandler(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        ILogger<CreateUserCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<Result<CreateUserResponse>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        try
        {
            // Check if email already exists
            if (await _unitOfWork.Users.EmailExistsAsync(request.Email, cancellationToken))
            {
                return Result<CreateUserResponse>.Failure("Email already exists");
            }

            // Check if login already exists
            if (await _unitOfWork.Users.LoginExistsAsync(request.Login, cancellationToken))
            {
                return Result<CreateUserResponse>.Failure("Login already exists");
            }

            // Create new user
            var user = new User
            {
                Name = request.Name,
                Email = request.Email,
                Login = request.Login,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
                Phone = request.Phone,
                Department = request.Department,
                Position = request.Position,
                IsActive = true
            };

            var createdUser = await _unitOfWork.Users.AddAsync(user, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var response = _mapper.Map<CreateUserResponse>(createdUser);
            
            _logger.LogInformation("User created successfully with ID: {UserId}", createdUser.Id);
            
            return Result<CreateUserResponse>.Success(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating user");
            return Result<CreateUserResponse>.Failure("An error occurred while creating the user");
        }
    }
}