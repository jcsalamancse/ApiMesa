using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MesaApi.Application.Features.Stats.Queries.GetDashboardStats;

namespace MesaApi.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class StatsController : ControllerBase
{
    private readonly IMediator _mediator;

    public StatsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Get dashboard statistics
    /// </summary>
    /// <returns>Dashboard statistics</returns>
    [HttpGet("dashboard")]
    public async Task<IActionResult> GetDashboardStats()
    {
        var query = new GetDashboardStatsQuery();
        var result = await _mediator.Send(query);
        
        if (!result.IsSuccess)
        {
            return BadRequest(new { message = result.Error });
        }

        return Ok(result.Data);
    }

    /// <summary>
    /// Get request statistics by status
    /// </summary>
    /// <param name="startDate">Start date for statistics</param>
    /// <param name="endDate">End date for statistics</param>
    /// <returns>Request statistics by status</returns>
    [HttpGet("requests/status")]
    public async Task<IActionResult> GetRequestStatsByStatus(
        [FromQuery] DateTime? startDate = null, 
        [FromQuery] DateTime? endDate = null)
    {
        // TODO: Implement GetRequestStatsByStatus query
        return Ok(new { message = "Get request stats by status - to be implemented" });
    }

    /// <summary>
    /// Get request statistics by category
    /// </summary>
    /// <param name="startDate">Start date for statistics</param>
    /// <param name="endDate">End date for statistics</param>
    /// <returns>Request statistics by category</returns>
    [HttpGet("requests/category")]
    public async Task<IActionResult> GetRequestStatsByCategory(
        [FromQuery] DateTime? startDate = null, 
        [FromQuery] DateTime? endDate = null)
    {
        // TODO: Implement GetRequestStatsByCategory query
        return Ok(new { message = "Get request stats by category - to be implemented" });
    }

    /// <summary>
    /// Get user activity statistics
    /// </summary>
    /// <param name="startDate">Start date for statistics</param>
    /// <param name="endDate">End date for statistics</param>
    /// <returns>User activity statistics</returns>
    [HttpGet("users/activity")]
    [Authorize(Roles = "Administrador")]
    public async Task<IActionResult> GetUserActivityStats(
        [FromQuery] DateTime? startDate = null, 
        [FromQuery] DateTime? endDate = null)
    {
        // TODO: Implement GetUserActivityStats query
        return Ok(new { message = "Get user activity stats - to be implemented" });
    }

    /// <summary>
    /// Get response time statistics
    /// </summary>
    /// <param name="startDate">Start date for statistics</param>
    /// <param name="endDate">End date for statistics</param>
    /// <returns>Response time statistics</returns>
    [HttpGet("performance/response-time")]
    [Authorize(Roles = "Administrador,Técnico")]
    public async Task<IActionResult> GetResponseTimeStats(
        [FromQuery] DateTime? startDate = null, 
        [FromQuery] DateTime? endDate = null)
    {
        // TODO: Implement GetResponseTimeStats query
        return Ok(new { message = "Get response time stats - to be implemented" });
    }
}