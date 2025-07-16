using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MesaApi.Application.Common.Models;
using MesaApi.Domain.Enums;
using MesaApi.Infrastructure.Data;

namespace MesaApi.Application.Features.Reports.Queries.GetRequestsReport;

public class GetRequestsReportQueryHandler : IRequestHandler<GetRequestsReportQuery, Result<RequestsReportDto>>
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<GetRequestsReportQueryHandler> _logger;

    public GetRequestsReportQueryHandler(
        ApplicationDbContext context,
        ILogger<GetRequestsReportQueryHandler> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<Result<RequestsReportDto>> Handle(GetRequestsReportQuery request, CancellationToken cancellationToken)
    {
        try
        {
            // Set default date range if not provided
            var startDate = request.StartDate ?? DateTime.UtcNow.AddMonths(-1);
            var endDate = request.EndDate ?? DateTime.UtcNow;

            // Build query
            var query = _context.Requests
                .Include(r => r.Requester)
                .Include(r => r.AssignedTo)
                .Include(r => r.Comments)
                .Where(r => r.CreatedAt >= startDate && r.CreatedAt <= endDate)
                .AsQueryable();

            // Apply filters
            if (request.Status.HasValue)
            {
                query = query.Where(r => r.Status == request.Status.Value);
            }

            if (request.Priority.HasValue)
            {
                query = query.Where(r => r.Priority == request.Priority.Value);
            }

            if (!string.IsNullOrWhiteSpace(request.Category))
            {
                query = query.Where(r => r.Category == request.Category);
            }

            if (request.RequesterId.HasValue)
            {
                query = query.Where(r => r.RequesterId == request.RequesterId.Value);
            }

            if (request.AssignedToId.HasValue)
            {
                query = query.Where(r => r.AssignedToId == request.AssignedToId.Value);
            }

            // Execute query
            var requests = await query.ToListAsync(cancellationToken);

            // Create report items
            var reportItems = requests.Select(r => new RequestsReportItemDto(
                Id: r.Id,
                Description: r.Description.Length > 100 ? r.Description.Substring(0, 97) + "..." : r.Description,
                Status: r.Status.ToString(),
                Priority: r.Priority.ToString(),
                Category: r.Category,
                RequesterName: r.Requester.Name,
                AssignedToName: r.AssignedTo?.Name,
                CreatedAt: r.CreatedAt,
                CompletedAt: r.CompletedAt,
                ResolutionTime: r.CompletedAt.HasValue ? r.CompletedAt.Value - r.CreatedAt : null,
                CommentsCount: r.Comments.Count
            )).ToList();

            // Create summary
            var completedRequests = requests.Count(r => r.Status == RequestStatus.Completed);
            var pendingRequests = requests.Count(r => r.Status != RequestStatus.Completed);
            
            var resolutionTimes = requests
                .Where(r => r.CompletedAt.HasValue)
                .Select(r => (r.CompletedAt!.Value - r.CreatedAt).TotalHours)
                .ToList();
            
            var averageResolutionTime = resolutionTimes.Any() ? resolutionTimes.Average() : 0;

            var requestsByStatus = requests
                .GroupBy(r => r.Status)
                .ToDictionary(g => g.Key.ToString(), g => g.Count());

            var requestsByPriority = requests
                .GroupBy(r => r.Priority)
                .ToDictionary(g => g.Key.ToString(), g => g.Count());

            var requestsByCategory = requests
                .GroupBy(r => r.Category ?? "Uncategorized")
                .ToDictionary(g => g.Key, g => g.Count());

            var requestsByRequester = requests
                .GroupBy(r => r.Requester.Name)
                .ToDictionary(g => g.Key, g => g.Count());

            var requestsByAssignedTo = requests
                .Where(r => r.AssignedTo != null)
                .GroupBy(r => r.AssignedTo!.Name)
                .ToDictionary(g => g.Key, g => g.Count());

            var summary = new RequestsReportSummaryDto(
                TotalRequests: requests.Count,
                CompletedRequests: completedRequests,
                PendingRequests: pendingRequests,
                AverageResolutionTimeInHours: averageResolutionTime,
                RequestsByStatus: requestsByStatus,
                RequestsByPriority: requestsByPriority,
                RequestsByCategory: requestsByCategory,
                RequestsByRequester: requestsByRequester,
                RequestsByAssignedTo: requestsByAssignedTo
            );

            // Create report
            var report = new RequestsReportDto(
                StartDate: startDate,
                EndDate: endDate,
                Items: reportItems,
                Summary: summary
            );

            _logger.LogInformation("Generated requests report with {Count} items", reportItems.Count);
            
            return Result<RequestsReportDto>.Success(report);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error generating requests report");
            return Result<RequestsReportDto>.Failure("An error occurred while generating the requests report");
        }
    }
}