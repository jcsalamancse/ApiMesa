using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MesaApi.Application.Common.Models;
using MesaApi.Domain.Enums;
using MesaApi.Infrastructure.Data;

namespace MesaApi.Application.Features.Stats.Queries.GetDashboardStats;

public class GetDashboardStatsQueryHandler : IRequestHandler<GetDashboardStatsQuery, Result<DashboardStatsDto>>
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<GetDashboardStatsQueryHandler> _logger;

    public GetDashboardStatsQueryHandler(
        ApplicationDbContext context,
        ILogger<GetDashboardStatsQueryHandler> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<Result<DashboardStatsDto>> Handle(GetDashboardStatsQuery request, CancellationToken cancellationToken)
    {
        try
        {
            // Get request counts
            var totalCount = await _context.Requests.CountAsync(cancellationToken);
            var pendingCount = await _context.Requests.CountAsync(r => r.Status == RequestStatus.Pending, cancellationToken);
            var inProgressCount = await _context.Requests.CountAsync(r => r.Status == RequestStatus.InProgress, cancellationToken);
            var onHoldCount = await _context.Requests.CountAsync(r => r.Status == RequestStatus.OnHold, cancellationToken);
            var completedCount = await _context.Requests.CountAsync(r => r.Status == RequestStatus.Completed, cancellationToken);
            var cancelledCount = await _context.Requests.CountAsync(r => r.Status == RequestStatus.Cancelled, cancellationToken);
            var rejectedCount = await _context.Requests.CountAsync(r => r.Status == RequestStatus.Rejected, cancellationToken);

            var requestCounts = new RequestCountsDto(
                Total: totalCount,
                Pending: pendingCount,
                InProgress: inProgressCount,
                OnHold: onHoldCount,
                Completed: completedCount,
                Cancelled: cancelledCount,
                Rejected: rejectedCount
            );

            // Get requests by status
            var requestsByStatus = new List<RequestsByStatusDto>
            {
                new("Pending", pendingCount, CalculatePercentage(pendingCount, totalCount)),
                new("InProgress", inProgressCount, CalculatePercentage(inProgressCount, totalCount)),
                new("OnHold", onHoldCount, CalculatePercentage(onHoldCount, totalCount)),
                new("Completed", completedCount, CalculatePercentage(completedCount, totalCount)),
                new("Cancelled", cancelledCount, CalculatePercentage(cancelledCount, totalCount)),
                new("Rejected", rejectedCount, CalculatePercentage(rejectedCount, totalCount))
            };

            // Get requests by priority
            var requestsByPriority = await _context.Requests
                .GroupBy(r => r.Priority)
                .Select(g => new
                {
                    Priority = g.Key,
                    Count = g.Count()
                })
                .ToListAsync(cancellationToken);

            var requestsByPriorityDto = requestsByPriority
                .Select(r => new RequestsByPriorityDto(
                    Priority: r.Priority.ToString(),
                    Count: r.Count,
                    Percentage: CalculatePercentage(r.Count, totalCount)
                ))
                .ToList();

            // Get requests by category
            var requestsByCategory = await _context.Requests
                .Where(r => r.Category != null)
                .GroupBy(r => r.Category)
                .Select(g => new
                {
                    Category = g.Key,
                    Count = g.Count()
                })
                .ToListAsync(cancellationToken);

            var requestsByCategoryDto = requestsByCategory
                .Select(r => new RequestsByCategoryDto(
                    Category: r.Category ?? "Uncategorized",
                    Count: r.Count,
                    Percentage: CalculatePercentage(r.Count, totalCount)
                ))
                .ToList();

            // Add "Uncategorized" if there are requests without category
            var uncategorizedCount = totalCount - requestsByCategory.Sum(r => r.Count);
            if (uncategorizedCount > 0)
            {
                requestsByCategoryDto.Add(new RequestsByCategoryDto(
                    Category: "Uncategorized",
                    Count: uncategorizedCount,
                    Percentage: CalculatePercentage(uncategorizedCount, totalCount)
                ));
            }

            // Get recent activity (comments and status changes)
            var recentComments = await _context.Comments
                .Include(c => c.User)
                .Include(c => c.Request)
                .OrderByDescending(c => c.CreatedAt)
                .Take(10)
                .Select(c => new RecentActivityDto(
                    RequestId: c.RequestId,
                    Description: c.Content.Length > 50 ? c.Content.Substring(0, 47) + "..." : c.Content,
                    ActivityType: "Comment",
                    UserName: c.User.Name,
                    Timestamp: c.CreatedAt
                ))
                .ToListAsync(cancellationToken);

            var recentSteps = await _context.RequestSteps
                .Include(s => s.Request)
                .Include(s => s.AssignedTo)
                .Where(s => s.StepType == "StatusChange" || s.StepType == "Assignment")
                .OrderByDescending(s => s.CreatedAt)
                .Take(10)
                .Select(s => new RecentActivityDto(
                    RequestId: s.RequestId,
                    Description: s.StepName,
                    ActivityType: s.StepType ?? "Step",
                    UserName: s.AssignedTo != null ? s.AssignedTo.Name : "System",
                    Timestamp: s.CreatedAt
                ))
                .ToListAsync(cancellationToken);

            var recentActivity = recentComments.Concat(recentSteps)
                .OrderByDescending(a => a.Timestamp)
                .Take(10)
                .ToList();

            // Create response
            var response = new DashboardStatsDto(
                RequestCounts: requestCounts,
                RequestsByStatus: requestsByStatus,
                RequestsByPriority: requestsByPriorityDto,
                RequestsByCategory: requestsByCategoryDto,
                RecentActivity: recentActivity
            );

            _logger.LogInformation("Dashboard statistics retrieved successfully");
            
            return Result<DashboardStatsDto>.Success(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving dashboard statistics");
            return Result<DashboardStatsDto>.Failure("An error occurred while retrieving dashboard statistics");
        }
    }

    private double CalculatePercentage(int count, int total)
    {
        if (total == 0)
            return 0;

        return Math.Round((double)count / total * 100, 2);
    }
}