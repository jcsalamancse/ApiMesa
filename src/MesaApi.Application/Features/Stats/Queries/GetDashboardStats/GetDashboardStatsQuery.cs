using MediatR;
using MesaApi.Application.Common.Models;

namespace MesaApi.Application.Features.Stats.Queries.GetDashboardStats;

public record GetDashboardStatsQuery : IRequest<Result<DashboardStatsDto>>;

public record DashboardStatsDto(
    RequestCountsDto RequestCounts,
    List<RequestsByStatusDto> RequestsByStatus,
    List<RequestsByPriorityDto> RequestsByPriority,
    List<RequestsByCategoryDto> RequestsByCategory,
    List<RecentActivityDto> RecentActivity
);

public record RequestCountsDto(
    int Total,
    int Pending,
    int InProgress,
    int OnHold,
    int Completed,
    int Cancelled,
    int Rejected
);

public record RequestsByStatusDto(
    string Status,
    int Count,
    double Percentage
);

public record RequestsByPriorityDto(
    string Priority,
    int Count,
    double Percentage
);

public record RequestsByCategoryDto(
    string Category,
    int Count,
    double Percentage
);

public record RecentActivityDto(
    int RequestId,
    string Description,
    string ActivityType,
    string UserName,
    DateTime Timestamp
);