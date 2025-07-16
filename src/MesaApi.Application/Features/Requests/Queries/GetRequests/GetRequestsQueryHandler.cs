using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MesaApi.Application.Common.Models;
using MesaApi.Domain.Entities;
using MesaApi.Domain.Enums;
using MesaApi.Infrastructure.Data;

namespace MesaApi.Application.Features.Requests.Queries.GetRequests;

public class GetRequestsQueryHandler : IRequestHandler<GetRequestsQuery, Result<PagedResult<RequestListItemDto>>>
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<GetRequestsQueryHandler> _logger;

    public GetRequestsQueryHandler(
        ApplicationDbContext context,
        ILogger<GetRequestsQueryHandler> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<Result<PagedResult<RequestListItemDto>>> Handle(GetRequestsQuery request, CancellationToken cancellationToken)
    {
        try
        {
            // Start with base query
            IQueryable<Request> requestsQuery = _context.Requests
                .Include(r => r.Requester)
                .Include(r => r.AssignedTo)
                .Include(r => r.Comments)
                .Include(r => r.Steps)
                .AsNoTracking();

            // Apply filters
            if (request.Status.HasValue)
            {
                requestsQuery = requestsQuery.Where(r => r.Status == request.Status.Value);
            }

            if (request.Priority.HasValue)
            {
                requestsQuery = requestsQuery.Where(r => r.Priority == request.Priority.Value);
            }

            if (!string.IsNullOrWhiteSpace(request.Category))
            {
                requestsQuery = requestsQuery.Where(r => r.Category == request.Category);
            }

            if (request.RequesterId.HasValue)
            {
                requestsQuery = requestsQuery.Where(r => r.RequesterId == request.RequesterId.Value);
            }

            if (request.AssignedToId.HasValue)
            {
                requestsQuery = requestsQuery.Where(r => r.AssignedToId == request.AssignedToId.Value);
            }

            if (request.StartDate.HasValue)
            {
                requestsQuery = requestsQuery.Where(r => r.CreatedAt >= request.StartDate.Value);
            }

            if (request.EndDate.HasValue)
            {
                requestsQuery = requestsQuery.Where(r => r.CreatedAt <= request.EndDate.Value);
            }

            if (!string.IsNullOrWhiteSpace(request.SearchTerm))
            {
                var searchTerm = request.SearchTerm.ToLower();
                requestsQuery = requestsQuery.Where(r => 
                    r.Description.ToLower().Contains(searchTerm) ||
                    (r.Category != null && r.Category.ToLower().Contains(searchTerm)) ||
                    (r.SubCategory != null && r.SubCategory.ToLower().Contains(searchTerm)) ||
                    r.Requester.Name.ToLower().Contains(searchTerm) ||
                    (r.AssignedTo != null && r.AssignedTo.Name.ToLower().Contains(searchTerm))
                );
            }

            // Get total count
            var totalCount = await requestsQuery.CountAsync(cancellationToken);

            // Apply pagination
            var pagedRequests = await requestsQuery
                .OrderByDescending(r => r.CreatedAt)
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToListAsync(cancellationToken);

            // Map to DTOs
            var requestDtos = pagedRequests.Select(r => new RequestListItemDto(
                Id: r.Id,
                Description: r.Description.Length > 100 ? r.Description.Substring(0, 97) + "..." : r.Description,
                Status: r.Status.ToString(),
                Priority: r.Priority.ToString(),
                Category: r.Category,
                SubCategory: r.SubCategory,
                RequesterId: r.RequesterId,
                RequesterName: r.Requester.Name,
                AssignedToId: r.AssignedToId,
                AssignedToName: r.AssignedTo?.Name,
                DueDate: r.DueDate,
                CreatedAt: r.CreatedAt,
                CompletedAt: r.CompletedAt,
                CommentsCount: r.Comments.Count,
                StepsCount: r.Steps.Count,
                CompletedStepsCount: r.Steps.Count(s => s.Status == StepStatus.Completed)
            )).ToList();

            var result = new PagedResult<RequestListItemDto>(
                requestDtos,
                totalCount,
                request.PageNumber,
                request.PageSize
            );

            _logger.LogInformation("Retrieved {Count} requests from page {PageNumber}", requestDtos.Count, request.PageNumber);
            
            return Result<PagedResult<RequestListItemDto>>.Success(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving requests");
            return Result<PagedResult<RequestListItemDto>>.Failure("An error occurred while retrieving requests");
        }
    }
}