using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MesaApi.Application.Features.Reports.Queries.GetRequestsReport;
using MesaApi.Domain.Enums;
using MesaApi.Application.Common.Interfaces;

namespace MesaApi.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ReportsController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IPdfGeneratorService _pdfGeneratorService;

    public ReportsController(
        IMediator mediator,
        IPdfGeneratorService pdfGeneratorService)
    {
        _mediator = mediator;
        _pdfGeneratorService = pdfGeneratorService;
    }

    /// <summary>
    /// Get requests report with filtering and grouping options
    /// </summary>
    /// <param name="startDate">Start date for report</param>
    /// <param name="endDate">End date for report</param>
    /// <param name="status">Filter by status</param>
    /// <param name="priority">Filter by priority</param>
    /// <param name="category">Filter by category</param>
    /// <param name="requesterId">Filter by requester ID</param>
    /// <param name="assignedToId">Filter by assigned user ID</param>
    /// <param name="groupBy">Group results by field (status, priority, category, requester, assignedTo)</param>
    /// <returns>Requests report</returns>
    [HttpGet("requests")]
    public async Task<IActionResult> GetRequestsReport(
        [FromQuery] DateTime? startDate = null,
        [FromQuery] DateTime? endDate = null,
        [FromQuery] RequestStatus? status = null,
        [FromQuery] RequestPriority? priority = null,
        [FromQuery] string? category = null,
        [FromQuery] int? requesterId = null,
        [FromQuery] int? assignedToId = null,
        [FromQuery] string? groupBy = null)
    {
        var query = new GetRequestsReportQuery(
            StartDate: startDate,
            EndDate: endDate,
            Status: status,
            Priority: priority,
            Category: category,
            RequesterId: requesterId,
            AssignedToId: assignedToId,
            GroupBy: groupBy
        );

        var result = await _mediator.Send(query);
        
        if (!result.IsSuccess)
        {
            return BadRequest(new { message = result.Error });
        }

        return Ok(result.Data);
    }

    /// <summary>
    /// Export requests report to PDF
    /// </summary>
    /// <param name="startDate">Start date for report</param>
    /// <param name="endDate">End date for report</param>
    /// <param name="status">Filter by status</param>
    /// <param name="priority">Filter by priority</param>
    /// <param name="category">Filter by category</param>
    /// <param name="requesterId">Filter by requester ID</param>
    /// <param name="assignedToId">Filter by assigned user ID</param>
    /// <returns>PDF file</returns>
    [HttpGet("requests/pdf")]
    public async Task<IActionResult> ExportRequestsReportToPdf(
        [FromQuery] DateTime? startDate = null,
        [FromQuery] DateTime? endDate = null,
        [FromQuery] RequestStatus? status = null,
        [FromQuery] RequestPriority? priority = null,
        [FromQuery] string? category = null,
        [FromQuery] int? requesterId = null,
        [FromQuery] int? assignedToId = null)
    {
        try
        {
            // First get the report data
            var query = new GetRequestsReportQuery(
                StartDate: startDate,
                EndDate: endDate,
                Status: status,
                Priority: priority,
                Category: category,
                RequesterId: requesterId,
                AssignedToId: assignedToId
            );

            var result = await _mediator.Send(query);
            
            if (!result.IsSuccess)
            {
                return BadRequest(new { message = result.Error });
            }

            // Generate PDF from report data
            var title = "Requests Report";
            if (startDate.HasValue && endDate.HasValue)
            {
                title += $" ({startDate.Value:yyyy-MM-dd} to {endDate.Value:yyyy-MM-dd})";
            }
            
            var pdfBytes = await _pdfGeneratorService.GenerateReportPdfAsync(result.Data, title);
            return File(pdfBytes, "application/pdf", "requests-report.pdf");
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = $"Error generating PDF: {ex.Message}" });
        }
    }

    /// <summary>
    /// Export requests report to Excel
    /// </summary>
    /// <param name="startDate">Start date for report</param>
    /// <param name="endDate">End date for report</param>
    /// <param name="status">Filter by status</param>
    /// <param name="priority">Filter by priority</param>
    /// <param name="category">Filter by category</param>
    /// <param name="requesterId">Filter by requester ID</param>
    /// <param name="assignedToId">Filter by assigned user ID</param>
    /// <returns>Excel file</returns>
    [HttpGet("requests/excel")]
    public async Task<IActionResult> ExportRequestsReportToExcel(
        [FromQuery] DateTime? startDate = null,
        [FromQuery] DateTime? endDate = null,
        [FromQuery] RequestStatus? status = null,
        [FromQuery] RequestPriority? priority = null,
        [FromQuery] string? category = null,
        [FromQuery] int? requesterId = null,
        [FromQuery] int? assignedToId = null)
    {
        // TODO: Implement export to Excel functionality
        return Ok(new { message = "Export requests report to Excel - to be implemented" });
    }

    /// <summary>
    /// Get user performance report
    /// </summary>
    /// <param name="startDate">Start date for report</param>
    /// <param name="endDate">End date for report</param>
    /// <param name="userId">Filter by user ID</param>
    /// <returns>User performance report</returns>
    [HttpGet("users/performance")]
    [Authorize(Roles = "Administrador")]
    public async Task<IActionResult> GetUserPerformanceReport(
        [FromQuery] DateTime? startDate = null,
        [FromQuery] DateTime? endDate = null,
        [FromQuery] int? userId = null)
    {
        // TODO: Implement user performance report
        return Ok(new { message = "Get user performance report - to be implemented" });
    }
}