using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MesaApi.Application.Common.Models;
using MesaApi.Infrastructure.Data;

namespace MesaApi.Application.Features.Workflows.Queries.GetWorkflows;

public class GetWorkflowsQueryHandler : IRequestHandler<GetWorkflowsQuery, Result<List<WorkflowDto>>>
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<GetWorkflowsQueryHandler> _logger;

    public GetWorkflowsQueryHandler(
        ApplicationDbContext context,
        ILogger<GetWorkflowsQueryHandler> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<Result<List<WorkflowDto>>> Handle(GetWorkflowsQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var query = _context.Workflows
                .Include(w => w.Steps)
                .AsQueryable();

            // Apply filters
            if (request.IsActive.HasValue)
            {
                query = query.Where(w => w.IsActive == request.IsActive.Value);
            }

            if (!string.IsNullOrWhiteSpace(request.Category))
            {
                query = query.Where(w => w.Category == request.Category);
            }

            // Execute query
            var workflows = await query
                .OrderBy(w => w.Name)
                .Select(w => new WorkflowDto(
                    Id: w.Id,
                    Name: w.Name,
                    Description: w.Description,
                    Category: w.Category,
                    IsActive: w.IsActive,
                    StepsCount: w.Steps.Count,
                    CreatedAt: w.CreatedAt,
                    CreatedBy: w.CreatedBy ?? "System"
                ))
                .ToListAsync(cancellationToken);

            _logger.LogInformation("Retrieved {Count} workflows", workflows.Count);
            
            return Result<List<WorkflowDto>>.Success(workflows);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving workflows");
            return Result<List<WorkflowDto>>.Failure("An error occurred while retrieving workflows");
        }
    }
}