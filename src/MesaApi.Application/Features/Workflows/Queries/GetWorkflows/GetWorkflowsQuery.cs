using MediatR;
using MesaApi.Application.Common.Models;

namespace MesaApi.Application.Features.Workflows.Queries.GetWorkflows;

public record GetWorkflowsQuery(
    bool? IsActive = null,
    string? Category = null
) : IRequest<Result<List<WorkflowDto>>>;

public record WorkflowDto(
    int Id,
    string Name,
    string Description,
    string Category,
    bool IsActive,
    int StepsCount,
    DateTime CreatedAt,
    string CreatedBy
);