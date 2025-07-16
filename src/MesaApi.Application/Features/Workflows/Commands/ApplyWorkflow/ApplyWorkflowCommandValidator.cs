using FluentValidation;

namespace MesaApi.Application.Features.Workflows.Commands.ApplyWorkflow;

public class ApplyWorkflowCommandValidator : AbstractValidator<ApplyWorkflowCommand>
{
    public ApplyWorkflowCommandValidator()
    {
        RuleFor(x => x.RequestId)
            .GreaterThan(0).WithMessage("RequestId must be greater than 0");

        RuleFor(x => x.WorkflowId)
            .GreaterThan(0).WithMessage("WorkflowId must be greater than 0");
    }
}