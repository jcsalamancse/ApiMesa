using MediatR;
using MesaApi.Application.Common.Models;

namespace MesaApi.Application.Features.Workflows.Queries.GetWorkflowById;

public record GetWorkflowByIdQuery(int Id) : IRequest<Result<WorkflowDetailDto>>;

public record WorkflowDetailDto(
    int Id,
    string Name,
    string Description,
    string Category,
    bool IsActive,
    List<WorkflowStepDto> Steps,
    DateTime CreatedAt,
    string CreatedBy,
    DateTime? UpdatedAt,
    string? UpdatedBy
);

public record WorkflowStepDto(
    int Id,
    string StepName,
    string? StepType,
    int Order,
    int? RoleId,
    string? RoleName
);