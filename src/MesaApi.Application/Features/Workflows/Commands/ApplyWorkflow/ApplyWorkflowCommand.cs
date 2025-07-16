using MediatR;
using MesaApi.Application.Common.Models;

namespace MesaApi.Application.Features.Workflows.Commands.ApplyWorkflow;

public record ApplyWorkflowCommand(
    int RequestId,
    int WorkflowId
) : IRequest<Result<ApplyWorkflowResponse>>;

public record ApplyWorkflowResponse(
    int RequestId,
    int WorkflowId,
    string WorkflowName,
    List<AppliedStepDto> AppliedSteps,
    DateTime AppliedAt,
    string AppliedBy
);

public record AppliedStepDto(
    int Id,
    string StepName,
    string? StepType,
    int Order,
    string Status,
    int? RoleId,
    string? RoleName
);