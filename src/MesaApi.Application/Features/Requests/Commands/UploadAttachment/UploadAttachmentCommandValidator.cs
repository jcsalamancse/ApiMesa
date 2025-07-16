using FluentValidation;
using Microsoft.AspNetCore.Http;

namespace MesaApi.Application.Features.Requests.Commands.UploadAttachment;

public class UploadAttachmentCommandValidator : AbstractValidator<UploadAttachmentCommand>
{
    public UploadAttachmentCommandValidator()
    {
        RuleFor(x => x.RequestId)
            .GreaterThan(0).WithMessage("RequestId must be greater than 0");

        RuleFor(x => x.File)
            .NotNull().WithMessage("File is required")
            .Must(BeValidFile).WithMessage("Invalid file. Maximum file size is 10MB and allowed file types are: pdf, doc, docx, xls, xlsx, ppt, pptx, txt, csv, jpg, jpeg, png, gif");

        RuleFor(x => x.Description)
            .MaximumLength(500).WithMessage("Description must not exceed 500 characters")
            .When(x => !string.IsNullOrEmpty(x.Description));
    }

    private bool BeValidFile(IFormFile file)
    {
        if (file == null)
            return false;

        // Check file size (max 10MB)
        if (file.Length > 10 * 1024 * 1024)
            return false;

        // Check file extension
        var allowedExtensions = new[] { ".pdf", ".doc", ".docx", ".xls", ".xlsx", ".ppt", ".pptx", ".txt", ".csv", ".jpg", ".jpeg", ".png", ".gif" };
        var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
        
        return allowedExtensions.Contains(extension);
    }
}