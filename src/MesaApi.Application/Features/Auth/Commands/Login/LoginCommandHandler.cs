using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using MesaApi.Application.Common.Interfaces;
using MesaApi.Application.Common.Models;
using MesaApi.Domain.Entities;
using MesaApi.Domain.Interfaces;
using BCrypt.Net;

namespace MesaApi.Application.Features.Auth.Commands.Login;

public class LoginCommandHandler : IRequestHandler<LoginCommand, Result<LoginResponse>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IJwtTokenService _jwtTokenService;
    private readonly IMapper _mapper;
    private readonly ILogger<LoginCommandHandler> _logger;

    public LoginCommandHandler(
        IUnitOfWork unitOfWork,
        IJwtTokenService jwtTokenService,
        IMapper mapper,
        ILogger<LoginCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _jwtTokenService = jwtTokenService;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<Result<LoginResponse>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        try
        {
            // Find user by login
            var user = await _unitOfWork.Users.GetByLoginAsync(request.Login, cancellationToken);
            
            if (user == null)
            {
                _logger.LogWarning("Login attempt failed: User {Login} not found", request.Login);
                return Result<LoginResponse>.Failure("Invalid login credentials");
            }

            // Verify password
            if (!BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            {
                _logger.LogWarning("Login attempt failed: Invalid password for user {Login}", request.Login);
                return Result<LoginResponse>.Failure("Invalid login credentials");
            }

            // Check if user is active
            if (!user.IsActive)
            {
                _logger.LogWarning("Login attempt failed: User {Login} is inactive", request.Login);
                return Result<LoginResponse>.Failure("User account is inactive");
            }

            // Generate JWT token
            var token = _jwtTokenService.GenerateToken(user);
            var refreshToken = _jwtTokenService.GenerateRefreshToken();

            // Create user session
            var session = new UserSession
            {
                UserId = user.Id,
                Login = user.Login,
                SessionId = refreshToken,
                ExpiresAt = DateTime.UtcNow.AddDays(7),
                IsActive = true
            };

            await _unitOfWork.UserSessions.AddAsync(session, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            // Create response
            var userInfo = _mapper.Map<UserInfo>(user);
            var response = new LoginResponse(
                Token: token,
                RefreshToken: refreshToken,
                ExpiresAt: DateTime.UtcNow.AddHours(1),
                User: userInfo
            );

            _logger.LogInformation("User {Login} logged in successfully", user.Login);
            
            return Result<LoginResponse>.Success(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during login for user {Login}", request.Login);
            return Result<LoginResponse>.Failure("An error occurred during login");
        }
    }
}