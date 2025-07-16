using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MesaApi.Application.Features.Roles.Queries.GetRoles;
using MesaApi.Application.Features.Roles.Queries.GetRoleById;

namespace MesaApi.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class RolesController : ControllerBase
{
    private readonly IMediator _mediator;

    public RolesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Get all roles
    /// </summary>
    /// <param name="isActive">Filter by active status</param>
    /// <returns>List of roles</returns>
    [HttpGet]
    public async Task<IActionResult> GetRoles([FromQuery] bool? isActive = null)
    {
        var query = new GetRolesQuery(isActive);
        var result = await _mediator.Send(query);
        
        if (!result.IsSuccess)
        {
            return BadRequest(new { message = result.Error });
        }

        return Ok(result.Data);
    }

    /// <summary>
    /// Get role by ID
    /// </summary>
    /// <param name="id">Role ID</param>
    /// <returns>Role details</returns>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetRole(int id)
    {
        var query = new GetRoleByIdQuery(id);
        var result = await _mediator.Send(query);
        
        if (!result.IsSuccess)
        {
            return NotFound(new { message = result.Error });
        }

        return Ok(result.Data);
    }

    /// <summary>
    /// Create a new role
    /// </summary>
    /// <param name="command">Role creation data</param>
    /// <returns>Created role information</returns>
    [HttpPost]
    [Authorize(Roles = "Administrador")]
    public async Task<IActionResult> CreateRole([FromBody] object command)
    {
        // TODO: Implement CreateRole command
        return Ok(new { message = "Create role - to be implemented" });
    }

    /// <summary>
    /// Update role
    /// </summary>
    /// <param name="id">Role ID</param>
    /// <param name="command">Update data</param>
    /// <returns>Updated role information</returns>
    [HttpPut("{id}")]
    [Authorize(Roles = "Administrador")]
    public async Task<IActionResult> UpdateRole(int id, [FromBody] object command)
    {
        // TODO: Implement UpdateRole command
        return Ok(new { message = $"Update role {id} - to be implemented" });
    }

    /// <summary>
    /// Delete role (soft delete)
    /// </summary>
    /// <param name="id">Role ID</param>
    /// <returns>Success message</returns>
    [HttpDelete("{id}")]
    [Authorize(Roles = "Administrador")]
    public async Task<IActionResult> DeleteRole(int id)
    {
        // TODO: Implement DeleteRole command
        return Ok(new { message = $"Delete role {id} - to be implemented" });
    }
}