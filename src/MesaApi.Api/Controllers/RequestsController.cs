using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MesaApi.Application.Features.Requests.Commands.CreateRequest;
using MesaApi.Application.Features.Requests.Commands.UpdateRequestStatus;
using MesaApi.Application.Features.Requests.Commands.AssignRequest;
using MesaApi.Application.Features.Requests.Queries.GetRequestById;
using MesaApi.Application.Features.Requests.Queries.GetRequests;
using MesaApi.Application.Features.Comments.Commands.AddComment;
using MesaApi.Domain.Enums;
using MesaApi.Application.Common.Interfaces;

namespace MesaApi.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class RequestsController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IPdfGeneratorService _pdfGeneratorService;

    public RequestsController(
        IMediator mediator,
        IPdfGeneratorService pdfGeneratorService)
    {
        _mediator = mediator;
        _pdfGeneratorService = pdfGeneratorService;
    }

    /// <summary>
    /// Get all requests with pagination and filtering
    /// </summary>
    /// <param name="pageNumber">Page number</param>
    /// <param name="pageSize">Page size</param>
    /// <param name="status">Filter by status</param>
    /// <param name="priority">Filter by priority</param>
    /// <param name="category">Filter by category</param>
    /// <param name="requesterId">Filter by requester ID</param>
    /// <param name="assignedToId">Filter by assigned user ID</param>
    /// <param name="startDate">Filter by start date</param>
    /// <param name="endDate">Filter by end date</param>
    /// <param name="searchTerm">Search term for description, category, or names</param>
    /// <returns>Paginated list of requests</returns>
    [HttpGet]
    public async Task<IActionResult> GetRequests(
        [FromQuery] int pageNumber = 1, 
        [FromQuery] int pageSize = 10,
        [FromQuery] RequestStatus? status = null,
        [FromQuery] RequestPriority? priority = null,
        [FromQuery] string? category = null,
        [FromQuery] int? requesterId = null,
        [FromQuery] int? assignedToId = null,
        [FromQuery] DateTime? startDate = null,
        [FromQuery] DateTime? endDate = null,
        [FromQuery] string? searchTerm = null)
    {
        var query = new GetRequestsQuery(
            PageNumber: pageNumber,
            PageSize: pageSize,
            Status: status,
            Priority: priority,
            Category: category,
            RequesterId: requesterId,
            AssignedToId: assignedToId,
            StartDate: startDate,
            EndDate: endDate,
            SearchTerm: searchTerm
        );

        var result = await _mediator.Send(query);
        
        if (!result.IsSuccess)
        {
            return BadRequest(new { message = result.Error, errors = result.Errors });
        }

        return Ok(result.Data);
    }

    /// <summary>
    /// Get request by ID with all details
    /// </summary>
    /// <param name="id">Request ID</param>
    /// <returns>Request details</returns>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetRequest(int id)
    {
        var query = new GetRequestByIdQuery(id);
        var result = await _mediator.Send(query);
        
        if (!result.IsSuccess)
        {
            return NotFound(new { message = result.Error });
        }

        return Ok(result.Data);
    }

    /// <summary>
    /// Create a new request
    /// </summary>
    /// <param name="command">Request creation data</param>
    /// <returns>Created request information</returns>
    [HttpPost]
    public async Task<IActionResult> CreateRequest([FromBody] CreateRequestCommand command)
    {
        var result = await _mediator.Send(command);
        
        if (!result.IsSuccess)
        {
            return BadRequest(new { message = result.Error, errors = result.Errors });
        }

        return CreatedAtAction(nameof(GetRequest), new { id = result.Data!.Id }, result.Data);
    }

    /// <summary>
    /// Add comment to request
    /// </summary>
    /// <param name="id">Request ID</param>
    /// <param name="command">Comment data</param>
    /// <returns>Created comment</returns>
    [HttpPost("{id}/comments")]
    public async Task<IActionResult> AddComment(int id, [FromBody] AddCommentCommand command)
    {
        if (id != command.RequestId)
        {
            return BadRequest(new { message = "Request ID in URL does not match request ID in body" });
        }

        var result = await _mediator.Send(command);
        
        if (!result.IsSuccess)
        {
            return BadRequest(new { message = result.Error, errors = result.Errors });
        }

        return Ok(result.Data);
    }

    /// <summary>
    /// Update request status
    /// </summary>
    /// <param name="id">Request ID</param>
    /// <param name="command">Status update data</param>
    /// <returns>Updated request</returns>
    [HttpPut("{id}/status")]
    public async Task<IActionResult> UpdateStatus(int id, [FromBody] UpdateRequestStatusCommand command)
    {
        if (id != command.RequestId)
        {
            return BadRequest(new { message = "Request ID in URL does not match request ID in body" });
        }

        var result = await _mediator.Send(command);
        
        if (!result.IsSuccess)
        {
            return BadRequest(new { message = result.Error, errors = result.Errors });
        }

        return Ok(result.Data);
    }

    /// <summary>
    /// Assign request to user
    /// </summary>
    /// <param name="id">Request ID</param>
    /// <param name="command">Assignment data</param>
    /// <returns>Updated request</returns>
    [HttpPut("{id}/assign")]
    public async Task<IActionResult> AssignRequest(int id, [FromBody] AssignRequestCommand command)
    {
        if (id != command.RequestId)
        {
            return BadRequest(new { message = "Request ID in URL does not match request ID in body" });
        }

        var result = await _mediator.Send(command);
        
        if (!result.IsSuccess)
        {
            return BadRequest(new { message = result.Error, errors = result.Errors });
        }

        return Ok(result.Data);
    }

    /// <summary>
    /// Export request to PDF
    /// </summary>
    /// <param name="id">Request ID</param>
    /// <returns>PDF file</returns>
    [HttpGet("{id}/pdf")]
    public async Task<IActionResult> ExportRequestToPdf(int id)
    {
        try
        {
            var pdfBytes = await _pdfGeneratorService.GenerateRequestPdfAsync(id);
            return File(pdfBytes, "application/pdf", $"request-{id}.pdf");
        }
        catch (KeyNotFoundException)
        {
            return NotFound(new { message = $"Request with ID {id} not found" });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = $"Error generating PDF: {ex.Message}" });
        }
    }
}