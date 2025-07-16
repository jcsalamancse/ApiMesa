using FluentValidation;

namespace MesaApi.Application.Features.Workflows.Commands.CreateWorkflow;

public class CreateWorkflowCommandValidator : AbstractValidator<CreateWorkflowCommand>
{
    public CreateWorkflowCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required")
            .MaximumLength(100).WithMessage("Name must not exceed 100 characters");

        RuleFor(x => x.Description)
            .MaximumLength(500).WithMessage("Description must not exceed 500 characters");

        RuleFor(x => x.Category)
            .NotEmpty().WithMessage("Category is required")
            .MaximumLength(100).WithMessage("Category must not exceed 100 characters");

        RuleFor(x => x.Steps)
            .NotEmpty().WithMessage("At least one step is required");

        RuleForEach(x => x.Steps)
            .SetValidator(new WorkflowStepDtoValidator());
    }
}

public class WorkflowStepDtoValidator : AbstractValidator<WorkflowStepDto>
{
    public WorkflowStepDtoValidator()
    {
        RuleFor(x => x.StepName)
            .NotEmpty().WithMessage("Step name is required")
            .MaximumLength(100).WithMessage("Step name must not exceed 100 characters");

        RuleFor(x => x.StepType)
            .MaximumLength(50).WithMessage("Step type must not exceed 50 characters")
            .When(x => !string.IsNullOrEmpty(x.StepType));

        RuleFor(x => x.Order)
            .GreaterThan(0).WithMessage("Order must be greater than 0");
    }
}