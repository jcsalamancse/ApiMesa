using MediatR;
using MesaApi.Application.Common.Models;
using MesaApi.Domain.Enums;

namespace MesaApi.Application.Features.Requests.Queries.GetRequests;

public record GetRequestsQuery(
    int PageNumber = 1,
    int PageSize = 10,
    RequestStatus? Status = null,
    RequestPriority? Priority = null,
    string? Category = null,
    int? RequesterId = null,
    int? AssignedToId = null,
    DateTime? StartDate = null,
    DateTime? EndDate = null,
    string? SearchTerm = null
) : IRequest<Result<PagedResult<RequestListItemDto>>>;

public record RequestListItemDto(
    int Id,
    string Description,
    string Status,
    string Priority,
    string? Category,
    string? SubCategory,
    int RequesterId,
    string RequesterName,
    int? AssignedToId,
    string? AssignedToName,
    DateTime? DueDate,
    DateTime CreatedAt,
    DateTime? CompletedAt,
    int CommentsCount,
    int StepsCount,
    int CompletedStepsCount
);