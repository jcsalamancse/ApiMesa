using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MesaApi.Application.Features.Workflows.Commands.CreateWorkflow;
using MesaApi.Application.Features.Workflows.Commands.ApplyWorkflow;
using MesaApi.Application.Features.Workflows.Queries.GetWorkflows;
using MesaApi.Application.Features.Workflows.Queries.GetWorkflowById;

namespace MesaApi.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class WorkflowsController : ControllerBase
{
    private readonly IMediator _mediator;

    public WorkflowsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Get all workflows
    /// </summary>
    /// <param name="isActive">Filter by active status</param>
    /// <param name="category">Filter by category</param>
    /// <returns>List of workflows</returns>
    [HttpGet]
    public async Task<IActionResult> GetWorkflows(
        [FromQuery] bool? isActive = null,
        [FromQuery] string? category = null)
    {
        var query = new GetWorkflowsQuery(isActive, category);
        var result = await _mediator.Send(query);
        
        if (!result.IsSuccess)
        {
            return BadRequest(new { message = result.Error });
        }

        return Ok(result.Data);
    }

    /// <summary>
    /// Get workflow by ID
    /// </summary>
    /// <param name="id">Workflow ID</param>
    /// <returns>Workflow details</returns>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetWorkflow(int id)
    {
        var query = new GetWorkflowByIdQuery(id);
        var result = await _mediator.Send(query);
        
        if (!result.IsSuccess)
        {
            return NotFound(new { message = result.Error });
        }

        return Ok(result.Data);
    }

    /// <summary>
    /// Create a new workflow
    /// </summary>
    /// <param name="command">Workflow creation data</param>
    /// <returns>Created workflow information</returns>
    [HttpPost]
    [Authorize(Roles = "Administrador")]
    public async Task<IActionResult> CreateWorkflow([FromBody] CreateWorkflowCommand command)
    {
        var result = await _mediator.Send(command);
        
        if (!result.IsSuccess)
        {
            return BadRequest(new { message = result.Error, errors = result.Errors });
        }

        return CreatedAtAction(nameof(GetWorkflow), new { id = result.Data!.Id }, result.Data);
    }

    /// <summary>
    /// Update workflow
    /// </summary>
    /// <param name="id">Workflow ID</param>
    /// <param name="command">Update data</param>
    /// <returns>Updated workflow information</returns>
    [HttpPut("{id}")]
    [Authorize(Roles = "Administrador")]
    public async Task<IActionResult> UpdateWorkflow(int id, [FromBody] object command)
    {
        // TODO: Implement UpdateWorkflow command
        return Ok(new { message = $"Update workflow {id} - to be implemented" });
    }

    /// <summary>
    /// Delete workflow (soft delete)
    /// </summary>
    /// <param name="id">Workflow ID</param>
    /// <returns>Success message</returns>
    [HttpDelete("{id}")]
    [Authorize(Roles = "Administrador")]
    public async Task<IActionResult> DeleteWorkflow(int id)
    {
        // TODO: Implement DeleteWorkflow command
        return Ok(new { message = $"Delete workflow {id} - to be implemented" });
    }

    /// <summary>
    /// Apply workflow to request
    /// </summary>
    /// <param name="requestId">Request ID</param>
    /// <param name="workflowId">Workflow ID</param>
    /// <returns>Updated request information</returns>
    [HttpPost("apply/{workflowId}/request/{requestId}")]
    [Authorize(Roles = "Administrador,Técnico")]
    public async Task<IActionResult> ApplyWorkflow(int requestId, int workflowId)
    {
        var command = new ApplyWorkflowCommand(requestId, workflowId);
        var result = await _mediator.Send(command);
        
        if (!result.IsSuccess)
        {
            return BadRequest(new { message = result.Error, errors = result.Errors });
        }

        return Ok(result.Data);
    }
}