using FluentValidation;

namespace MesaApi.Application.Features.Requests.Commands.AssignRequest;

public class AssignRequestCommandValidator : AbstractValidator<AssignRequestCommand>
{
    public AssignRequestCommandValidator()
    {
        RuleFor(x => x.RequestId)
            .GreaterThan(0).WithMessage("RequestId must be greater than 0");

        RuleFor(x => x.AssignedToId)
            .GreaterThan(0).WithMessage("AssignedToId must be greater than 0");

        RuleFor(x => x.Comment)
            .MaximumLength(2000).WithMessage("Comment must not exceed 2000 characters")
            .When(x => !string.IsNullOrEmpty(x.Comment));
    }
}