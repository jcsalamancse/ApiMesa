using MediatR;
using MesaApi.Application.Common.Models;

namespace MesaApi.Application.Features.Workflows.Commands.CreateWorkflow;

public record CreateWorkflowCommand(
    string Name,
    string Description,
    string Category,
    List<WorkflowStepDto> Steps,
    bool IsActive = true
) : IRequest<Result<CreateWorkflowResponse>>;

public record WorkflowStepDto(
    string StepName,
    string? StepType,
    int Order,
    int? RoleId = null
);

public record CreateWorkflowResponse(
    int Id,
    string Name,
    string Description,
    string Category,
    bool IsActive,
    List<WorkflowStepResponseDto> Steps,
    DateTime CreatedAt,
    string CreatedBy
);

public record WorkflowStepResponseDto(
    int Id,
    string StepName,
    string? StepType,
    int Order,
    int? RoleId,
    string? RoleName
);