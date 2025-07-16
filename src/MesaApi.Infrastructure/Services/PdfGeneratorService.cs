using Microsoft.Extensions.Logging;
using MesaApi.Application.Common.Interfaces;
using MesaApi.Domain.Interfaces;
using MesaApi.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace MesaApi.Infrastructure.Services;

public class PdfGeneratorService : IPdfGeneratorService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ApplicationDbContext _context;
    private readonly ILogger<PdfGeneratorService> _logger;

    public PdfGeneratorService(
        IUnitOfWork unitOfWork,
        ApplicationDbContext context,
        ILogger<PdfGeneratorService> logger)
    {
        _unitOfWork = unitOfWork;
        _context = context;
        _logger = logger;
    }

    public async Task<byte[]> GenerateRequestPdfAsync(int requestId, CancellationToken cancellationToken = default)
    {
        try
        {
            // Get request with all details
            var request = await _context.Requests
                .Include(r => r.Requester)
                .Include(r => r.AssignedTo)
                .Include(r => r.Steps)
                    .ThenInclude(s => s.AssignedTo)
                .Include(r => r.Steps)
                    .ThenInclude(s => s.Role)
                .Include(r => r.Comments)
                    .ThenInclude(c => c.User)
                .Include(r => r.RequestData)
                .Include(r => r.Attachments)
                .FirstOrDefaultAsync(r => r.Id == requestId, cancellationToken);

            if (request == null)
            {
                throw new KeyNotFoundException($"Request with ID {requestId} not found");
            }

            // Generate HTML content
            var html = new StringBuilder();
            html.AppendLine("<!DOCTYPE html>");
            html.AppendLine("<html>");
            html.AppendLine("<head>");
            html.AppendLine("<meta charset='UTF-8'>");
            html.AppendLine("<title>Request Details</title>");
            html.AppendLine("<style>");
            html.AppendLine("body { font-family: Arial, sans-serif; margin: 20px; }");
            html.AppendLine("h1 { color: #333366; }");
            html.AppendLine("h2 { color: #333366; margin-top: 20px; }");
            html.AppendLine("table { border-collapse: collapse; width: 100%; margin-bottom: 20px; }");
            html.AppendLine("th, td { border: 1px solid #ddd; padding: 8px; text-align: left; }");
            html.AppendLine("th { background-color: #f2f2f2; }");
            html.AppendLine(".header { display: flex; justify-content: space-between; align-items: center; }");
            html.AppendLine(".logo { height: 60px; }");
            html.AppendLine(".footer { margin-top: 30px; text-align: center; font-size: 12px; color: #666; }");
            html.AppendLine("</style>");
            html.AppendLine("</head>");
            html.AppendLine("<body>");

            // Header
            html.AppendLine("<div class='header'>");
            html.AppendLine("<div>");
            html.AppendLine($"<h1>Request #{request.Id}</h1>");
            html.AppendLine($"<p>Status: {request.Status}</p>");
            html.AppendLine($"<p>Created: {request.CreatedAt:yyyy-MM-dd HH:mm}</p>");
            html.AppendLine("</div>");
            html.AppendLine("<div>");
            html.AppendLine("<img class='logo' src='data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAMgAAAA8CAYAAAAjW/WRAAABhGlDQ1BJQ0MgcHJvZmlsZQAAKJF9kT1Iw0AcxV9TpSIVBzuIOGSoThZERRy1CkWoEGqFVh1MbvqhNGlIUlwcBdeCgx+LVQcXZ10dXAVB8APE1cVJ0UVK/F9SaBHjwXE/3t173L0DhGaVqWbPOKBqlpFOxMVcflUMvCKIAYQxJjErWVhJzXbcxNd9Db3a3WV4FvfdfQ5RpZAJBHzEc0wzbeIN4ulN2+C8TxxlJVklPiceM+mCxI9cVzx+41x0WeCZUTOTniOOEovFDlY6mJVMjXiKOKZqOuULOY9VzluctXKNte7JXxjKaytprtMMI4FlJJGCCBk1VFBFDDFa1TMZKE/7eELHH3H+ErkUclXAyLGAGjTIrh/8D353axYmJ9ykUBwIvNj2xzAQ2AUaDdv+PrbtxgngfwautJa/UgdmPkmvtbTIEdC3DVxctzR5D7jcAQafdMmQHMlPSygWgfcz+qY8EL4FetfdubXOcfoAZGhWSzfAwSEwWqTsdY93d7b39u+ZZv9+AFlNcp0X9aicAAAABmJLR0QA/wD/AP+gvaeTAAAACXBIWXMAAC4jAAAuIwF4pT92AAAAB3RJTUUH5gcQFTUOAFQMdQAAABl0RVh0Q29tbWVudABDcmVhdGVkIHdpdGggR0lNUFeBDhcAAAUISURBVHja7Z1NbBVFFMd/fS1tSUkpKQSoUCkfAUQhITEqCQlRCYkmJiYuXLhwYeLShYkrjTEaN8bEhRs3blgYjR9R/AJiQFRAqPIRKKWUQr9LS9/r83VxzuPO6/vqfcyZO3Pv/JNJ2r6ZO3Pn/O+ZM2fOnJnCGGMQQgTSYZsgRDwSiBApSCBCpCCBCJGCBCJEChKIEClIIEKkIIEIkYIEIkQKEogQKUggQqQggQiRggQiRAoSiBApSCBCpCCBCJGCBCJEChKIEClIIEKkIIEIkYIEIkQKEogQKUggQqQggQiRggQiRAoSiBApSCBCpCCBCJGCBCJEChKIEClIIEKkIIEIkYIEIkQKEogQKUggQqQggQiRggQiRAoSiBApSCBCpCCBCJGCBCJEChKIEClIIEKkIIEIkYIEIkQKEogQKUggQqQggQiRggQiRAoSiBApSCBCpCCBCJGCBCJEChKIEClIIEKkIIEIkYIEIkQKEogQKUggQqQggQiRggQiRAoSiBApSCBCpCCBCJGCBCJEChKIEClIIEKkIIEIkYIEIkQKEogQKUggQqQggQiRggQiRAoSiBApSCBCpCCBCJGCBCJEChKIEClIIEKkIIEIkYIEIkQKEogQKUggQqQggQiRggQiRAoSiBApSCBCpCCBCJGCBCJEChKIEClIIEKkIIEIkYIEIkQKEogQKUggQqQggQiRggQiRAoSiBApSCBCpCCBCJGCBCJEChKIEClIIEKkIIEIkYIEIkQKEogQKUggQqQggQiRggQiRAoSiBApSCBCpCCBCJGCBCJEChKIEClIIEKkIIEIkYIEIkQKEogQKUggQqQggQiRggQiRAoSiBApSCBCpCCBCJGCBCJEChKIEClIIEKkIIEIkYIEIkQKEogQKUggQqQggQiRggQiRAoSiBApSCBCpCCBCJGCBCJEChKIEClIIEKkIIEIkYIEIkQKEogQKUggQqQggQiRggQiRAoSiBApSCBCpCCBCJGCBCJEChKIEClIIEKkIIEIkYIEIkQKEogQKUggQqQggQiRggQiRAoSiBApSCBCpCCBCJGCBCJEChKIEClIIEKkIIEIkYIEIkQKEogQKUggQqQggQiRggQiRAoSiBApSCBCpCCBCJGCBCJEChKIEClIIEKkIIEIkYIEIkQKEogQKUggQqQggQiRggQiRAoSiBApSCBCpCCBCJGCBCJEChKIEClIIEKkIIEIkYIEIkQKEogQKUggQqQggQiRggQiRAoSiBApSCBCpCCBCJGCBCJEChKIEClIIEKkIIEIkYIEIkQKEogQKUggQqQggQiRggQiRAoSiBApSCBCpCCBCJGCBCJEChKIEClIIEKkIIEIkYIEIkQKEogQKUggQqQggQiRggQiRAoSiBApSCBCpCCBCJGCBCJEChKIEClIIEKkIIEIkYIEIkQKEogQKUggQqQggQiRggQiRAoSiBApSCBCpCCBCJGCBCJEChKIEClIIEKkIIEIkYIEIkQKEogQKUggQqQggQiRggQiRAoSiBApSCBCpCCBCJGCBCJEChKIEClIIEKkIIEIkYIEIkQKEogQKUggQqQggQiRggQiRAoSiBApSCBCpCCBCJGCBCJEChKIEClIIEKkIIEIkYIEIkQKEogQKUggQqQggQiRggQiRAoSiBApSCBCpCCBCJGCBCJEChKIEClIIEKkIIEIkYIEIkQKEogQKUggQqQggQiRggQiRAoSiBApSCBCpCCBCJGCBCJEChKIEClIIEKk8D9RIWkbbUrj+QAAAABJRU5ErkJggg==' alt='Logo'>");
            html.AppendLine("</div>");
            html.AppendLine("</div>");

            // Request details
            html.AppendLine("<h2>Request Information</h2>");
            html.AppendLine("<table>");
            html.AppendLine("<tr><th>ID</th><td>" + request.Id + "</td></tr>");
            html.AppendLine("<tr><th>Status</th><td>" + request.Status + "</td></tr>");
            html.AppendLine("<tr><th>Priority</th><td>" + request.Priority + "</td></tr>");
            html.AppendLine("<tr><th>Category</th><td>" + (request.Category ?? "N/A") + "</td></tr>");
            html.AppendLine("<tr><th>Subcategory</th><td>" + (request.SubCategory ?? "N/A") + "</td></tr>");
            html.AppendLine("<tr><th>Requester</th><td>" + request.Requester.Name + "</td></tr>");
            html.AppendLine("<tr><th>Assigned To</th><td>" + (request.AssignedTo?.Name ?? "Not assigned") + "</td></tr>");
            html.AppendLine("<tr><th>Created At</th><td>" + request.CreatedAt.ToString("yyyy-MM-dd HH:mm") + "</td></tr>");
            html.AppendLine("<tr><th>Due Date</th><td>" + (request.DueDate?.ToString("yyyy-MM-dd") ?? "Not set") + "</td></tr>");
            html.AppendLine("<tr><th>Completed At</th><td>" + (request.CompletedAt?.ToString("yyyy-MM-dd HH:mm") ?? "Not completed") + "</td></tr>");
            html.AppendLine("</table>");

            // Description
            html.AppendLine("<h2>Description</h2>");
            html.AppendLine("<p>" + request.Description.Replace("\n", "<br>") + "</p>");

            // Request data
            if (request.RequestData.Any())
            {
                html.AppendLine("<h2>Additional Information</h2>");
                html.AppendLine("<table>");
                html.AppendLine("<tr><th>Name</th><th>Value</th></tr>");
                foreach (var data in request.RequestData)
                {
                    html.AppendLine("<tr><td>" + data.Name + "</td><td>" + data.Value + "</td></tr>");
                }
                html.AppendLine("</table>");
            }

            // Steps
            if (request.Steps.Any())
            {
                html.AppendLine("<h2>Workflow Steps</h2>");
                html.AppendLine("<table>");
                html.AppendLine("<tr><th>Step</th><th>Status</th><th>Assigned To</th><th>Role</th><th>Completed At</th></tr>");
                foreach (var step in request.Steps.OrderBy(s => s.Order))
                {
                    html.AppendLine("<tr>");
                    html.AppendLine("<td>" + step.StepName + "</td>");
                    html.AppendLine("<td>" + step.Status + "</td>");
                    html.AppendLine("<td>" + (step.AssignedTo?.Name ?? "N/A") + "</td>");
                    html.AppendLine("<td>" + (step.Role?.Name ?? "N/A") + "</td>");
                    html.AppendLine("<td>" + (step.CompletedAt?.ToString("yyyy-MM-dd HH:mm") ?? "Not completed") + "</td>");
                    html.AppendLine("</tr>");
                }
                html.AppendLine("</table>");
            }

            // Comments
            if (request.Comments.Any())
            {
                html.AppendLine("<h2>Comments</h2>");
                foreach (var comment in request.Comments.OrderBy(c => c.CreatedAt))
                {
                    html.AppendLine("<div style='margin-bottom: 15px; border-bottom: 1px solid #eee; padding-bottom: 10px;'>");
                    html.AppendLine("<p><strong>" + comment.User.Name + "</strong> - " + comment.CreatedAt.ToString("yyyy-MM-dd HH:mm") + "</p>");
                    html.AppendLine("<p>" + comment.Content.Replace("\n", "<br>") + "</p>");
                    html.AppendLine("</div>");
                }
            }

            // Attachments
            if (request.Attachments.Any())
            {
                html.AppendLine("<h2>Attachments</h2>");
                html.AppendLine("<ul>");
                foreach (var attachment in request.Attachments)
                {
                    html.AppendLine("<li>" + attachment.FileName + " (" + FormatFileSize(attachment.FileSize) + ")</li>");
                }
                html.AppendLine("</ul>");
            }

            // Footer
            html.AppendLine("<div class='footer'>");
            html.AppendLine("<p>Generated on " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "</p>");
            html.AppendLine("<p>Mesa API - DataiFX</p>");
            html.AppendLine("</div>");

            html.AppendLine("</body>");
            html.AppendLine("</html>");

            // Convert HTML to PDF using a PDF library
            // For this example, we'll just return the HTML as bytes
            // In a real implementation, you would use a library like DinkToPdf, iText, or similar
            return Encoding.UTF8.GetBytes(html.ToString());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error generating PDF for request {RequestId}", requestId);
            throw;
        }
    }

    public async Task<byte[]> GenerateReportPdfAsync(object reportData, string reportTitle, CancellationToken cancellationToken = default)
    {
        // Similar implementation to GenerateRequestPdfAsync but for reports
        // For now, we'll return a simple placeholder
        var html = $"<html><body><h1>{reportTitle}</h1><p>Report data would be rendered here</p></body></html>";
        return Encoding.UTF8.GetBytes(html);
    }

    private string FormatFileSize(long bytes)
    {
        string[] sizes = { "B", "KB", "MB", "GB", "TB" };
        double len = bytes;
        int order = 0;
        while (len >= 1024 && order < sizes.Length - 1)
        {
            order++;
            len = len / 1024;
        }
        return $"{len:0.##} {sizes[order]}";
    }
}