using MediatR;
using Microsoft.Extensions.Logging;
using MesaApi.Application.Common.Models;
using MesaApi.Domain.Entities;
using MesaApi.Domain.Interfaces;

namespace MesaApi.Application.Features.Users.Queries.GetUsers;

public class GetUsersQueryHandler : IRequestHandler<GetUsersQuery, Result<PagedResult<UserListItemDto>>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<GetUsersQueryHandler> _logger;

    public GetUsersQueryHandler(
        IUnitOfWork unitOfWork,
        ILogger<GetUsersQueryHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Result<PagedResult<UserListItemDto>>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
    {
        try
        {
            // Get all users first
            var allUsers = await _unitOfWork.Users.GetAllAsync(cancellationToken);
            var usersQuery = allUsers.AsEnumerable();

            // Apply filters
            if (request.IsActive.HasValue)
            {
                usersQuery = usersQuery.Where(u => u.IsActive == request.IsActive.Value);
            }

            if (!string.IsNullOrWhiteSpace(request.Department))
            {
                usersQuery = usersQuery.Where(u => u.Department == request.Department);
            }

            if (!string.IsNullOrWhiteSpace(request.SearchTerm))
            {
                var searchTerm = request.SearchTerm.ToLower();
                usersQuery = usersQuery.Where(u => 
                    u.Name.ToLower().Contains(searchTerm) ||
                    u.Email.ToLower().Contains(searchTerm) ||
                    u.Login.ToLower().Contains(searchTerm) ||
                    (u.Department != null && u.Department.ToLower().Contains(searchTerm)) ||
                    (u.Position != null && u.Position.ToLower().Contains(searchTerm))
                );
            }

            // Get total count
            var totalCount = usersQuery.Count();

            // Apply pagination
            var pagedUsers = usersQuery
                .OrderBy(u => u.Name)
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToList();

            // Map to DTOs
            var userDtos = pagedUsers.Select(u => new UserListItemDto(
                Id: u.Id,
                Name: u.Name,
                Email: u.Email,
                Login: u.Login,
                Department: u.Department,
                Position: u.Position,
                IsActive: u.IsActive,
                CreatedAt: u.CreatedAt
            )).ToList();

            var result = new PagedResult<UserListItemDto>(
                userDtos,
                totalCount,
                request.PageNumber,
                request.PageSize
            );

            _logger.LogInformation("Retrieved {Count} users from page {PageNumber}", userDtos.Count, request.PageNumber);
            
            return Result<PagedResult<UserListItemDto>>.Success(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving users");
            return Result<PagedResult<UserListItemDto>>.Failure("An error occurred while retrieving users");
        }
    }
}