using MediatR;
using MesaApi.Application.Common.Models;
using MesaApi.Domain.Enums;

namespace MesaApi.Application.Features.Reports.Queries.GetRequestsReport;

public record GetRequestsReportQuery(
    DateTime? StartDate = null,
    DateTime? EndDate = null,
    RequestStatus? Status = null,
    RequestPriority? Priority = null,
    string? Category = null,
    int? RequesterId = null,
    int? AssignedToId = null,
    string? GroupBy = null
) : IRequest<Result<RequestsReportDto>>;

public record RequestsReportDto(
    DateTime StartDate,
    DateTime EndDate,
    List<RequestsReportItemDto> Items,
    RequestsReportSummaryDto Summary
);

public record RequestsReportItemDto(
    int Id,
    string Description,
    string Status,
    string Priority,
    string? Category,
    string RequesterName,
    string? AssignedToName,
    DateTime CreatedAt,
    DateTime? CompletedAt,
    TimeSpan? ResolutionTime,
    int CommentsCount
);

public record RequestsReportSummaryDto(
    int TotalRequests,
    int CompletedRequests,
    int PendingRequests,
    double AverageResolutionTimeInHours,
    Dictionary<string, int> RequestsByStatus,
    Dictionary<string, int> RequestsByPriority,
    Dictionary<string, int> RequestsByCategory,
    Dictionary<string, int> RequestsByRequester,
    Dictionary<string, int> RequestsByAssignedTo
);