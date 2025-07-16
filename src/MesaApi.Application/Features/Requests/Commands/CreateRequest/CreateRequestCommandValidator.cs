using FluentValidation;
using MesaApi.Domain.Enums;

namespace MesaApi.Application.Features.Requests.Commands.CreateRequest;

public class CreateRequestCommandValidator : AbstractValidator<CreateRequestCommand>
{
    public CreateRequestCommandValidator()
    {
        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Description is required")
            .MaximumLength(2000).WithMessage("Description must not exceed 2000 characters");

        RuleFor(x => x.Priority)
            .IsInEnum().WithMessage("Invalid priority value");

        RuleFor(x => x.Category)
            .MaximumLength(100).WithMessage("Category must not exceed 100 characters")
            .When(x => !string.IsNullOrEmpty(x.Category));

        RuleFor(x => x.SubCategory)
            .MaximumLength(100).WithMessage("SubCategory must not exceed 100 characters")
            .When(x => !string.IsNullOrEmpty(x.SubCategory));

        RuleFor(x => x.DueDate)
            .Must(BeAFutureDate).WithMessage("Due date must be in the future")
            .When(x => x.DueDate.HasValue);

        RuleForEach(x => x.RequestData)
            .SetValidator(new RequestDataDtoValidator())
            .When(x => x.RequestData != null && x.RequestData.Any());
    }

    private bool BeAFutureDate(DateTime? date)
    {
        return !date.HasValue || date.Value > DateTime.UtcNow;
    }
}

public class RequestDataDtoValidator : AbstractValidator<RequestDataDto>
{
    public RequestDataDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required")
            .MaximumLength(100).WithMessage("Name must not exceed 100 characters");

        RuleFor(x => x.Value)
            .NotEmpty().WithMessage("Value is required")
            .MaximumLength(500).WithMessage("Value must not exceed 500 characters");

        RuleFor(x => x.DataType)
            .MaximumLength(50).WithMessage("DataType must not exceed 50 characters")
            .When(x => !string.IsNullOrEmpty(x.DataType));
    }
}