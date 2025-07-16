using FluentValidation;
using MesaApi.Domain.Enums;

namespace MesaApi.Application.Features.Requests.Commands.UpdateRequestStatus;

public class UpdateRequestStatusCommandValidator : AbstractValidator<UpdateRequestStatusCommand>
{
    public UpdateRequestStatusCommandValidator()
    {
        RuleFor(x => x.RequestId)
            .GreaterThan(0).WithMessage("RequestId must be greater than 0");

        RuleFor(x => x.NewStatus)
            .IsInEnum().WithMessage("Invalid status value");

        RuleFor(x => x.Comment)
            .MaximumLength(2000).WithMessage("Comment must not exceed 2000 characters")
            .When(x => !string.IsNullOrEmpty(x.Comment));
    }
}