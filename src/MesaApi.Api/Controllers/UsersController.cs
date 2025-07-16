using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MesaApi.Application.Features.Users.Commands.CreateUser;
using MesaApi.Application.Features.Users.Commands.UpdateUser;
using MesaApi.Application.Features.Users.Commands.DeleteUser;
using MesaApi.Application.Features.Users.Commands.ChangePassword;
using MesaApi.Application.Features.Users.Queries.GetUserById;
using MesaApi.Application.Features.Users.Queries.GetUsers;
using System.Security.Claims;

namespace MesaApi.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class UsersController : ControllerBase
{
    private readonly IMediator _mediator;

    public UsersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Create a new user
    /// </summary>
    /// <param name="command">User creation data</param>
    /// <returns>Created user information</returns>
    [HttpPost]
    [Authorize(Roles = "Administrador")]
    public async Task<IActionResult> CreateUser([FromBody] CreateUserCommand command)
    {
        var result = await _mediator.Send(command);
        
        if (!result.IsSuccess)
        {
            return BadRequest(new { message = result.Error, errors = result.Errors });
        }

        return CreatedAtAction(nameof(GetUser), new { id = result.Data!.Id }, result.Data);
    }

    /// <summary>
    /// Get user by ID
    /// </summary>
    /// <param name="id">User ID</param>
    /// <returns>User information</returns>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetUser(int id)
    {
        var query = new GetUserByIdQuery(id);
        var result = await _mediator.Send(query);
        
        if (!result.IsSuccess)
        {
            return NotFound(new { message = result.Error });
        }

        return Ok(result.Data);
    }

    /// <summary>
    /// Get all users with pagination and filtering
    /// </summary>
    /// <param name="pageNumber">Page number</param>
    /// <param name="pageSize">Page size</param>
    /// <param name="isActive">Filter by active status</param>
    /// <param name="department">Filter by department</param>
    /// <param name="searchTerm">Search term for name, email, etc.</param>
    /// <returns>Paginated list of users</returns>
    [HttpGet]
    public async Task<IActionResult> GetUsers(
        [FromQuery] int pageNumber = 1, 
        [FromQuery] int pageSize = 10,
        [FromQuery] bool? isActive = null,
        [FromQuery] string? department = null,
        [FromQuery] string? searchTerm = null)
    {
        var query = new GetUsersQuery(
            PageNumber: pageNumber,
            PageSize: pageSize,
            IsActive: isActive,
            Department: department,
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
    /// Update user
    /// </summary>
    /// <param name="id">User ID</param>
    /// <param name="command">Update data</param>
    /// <returns>Updated user information</returns>
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateUser(int id, [FromBody] UpdateUserCommand command)
    {
        if (id != command.Id)
        {
            return BadRequest(new { message = "User ID in URL does not match user ID in body" });
        }

        // Check if user is updating their own profile or is an admin
        var currentUserIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var isAdmin = User.IsInRole("Administrador");
        
        if (!isAdmin && (!int.TryParse(currentUserIdClaim, out var currentUserId) || currentUserId != id))
        {
            return Forbid();
        }

        var result = await _mediator.Send(command);
        
        if (!result.IsSuccess)
        {
            return BadRequest(new { message = result.Error, errors = result.Errors });
        }

        return Ok(result.Data);
    }

    /// <summary>
    /// Delete user (soft delete)
    /// </summary>
    /// <param name="id">User ID</param>
    /// <returns>Success message</returns>
    [HttpDelete("{id}")]
    [Authorize(Roles = "Administrador")]
    public async Task<IActionResult> DeleteUser(int id)
    {
        var command = new DeleteUserCommand(id);
        var result = await _mediator.Send(command);
        
        if (!result.IsSuccess)
        {
            return BadRequest(new { message = result.Error });
        }

        return Ok(new { message = "User deleted successfully" });
    }

    /// <summary>
    /// Change user password
    /// </summary>
    /// <param name="id">User ID</param>
    /// <param name="command">Password change data</param>
    /// <returns>Success message</returns>
    [HttpPost("{id}/change-password")]
    public async Task<IActionResult> ChangePassword(int id, [FromBody] ChangePasswordCommand command)
    {
        if (id != command.UserId)
        {
            return BadRequest(new { message = "User ID in URL does not match user ID in body" });
        }

        // Check if user is changing their own password or is an admin
        var currentUserIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var isAdmin = User.IsInRole("Administrador");
        
        if (!isAdmin && (!int.TryParse(currentUserIdClaim, out var currentUserId) || currentUserId != id))
        {
            return Forbid();
        }

        var result = await _mediator.Send(command);
        
        if (!result.IsSuccess)
        {
            return BadRequest(new { message = result.Error });
        }

        return Ok(new { message = "Password changed successfully" });
    }

    /// <summary>
    /// Get current user profile
    /// </summary>
    /// <returns>Current user information</returns>
    [HttpGet("me")]
    public async Task<IActionResult> GetCurrentUser()
    {
        var currentUserIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        
        if (string.IsNullOrEmpty(currentUserIdClaim) || !int.TryParse(currentUserIdClaim, out var currentUserId))
        {
            return BadRequest(new { message = "Invalid user ID in token" });
        }

        var query = new GetUserByIdQuery(currentUserId);
        var result = await _mediator.Send(query);
        
        if (!result.IsSuccess)
        {
            return NotFound(new { message = result.Error });
        }

        return Ok(result.Data);
    }
}