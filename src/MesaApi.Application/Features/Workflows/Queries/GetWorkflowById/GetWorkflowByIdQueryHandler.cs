using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MesaApi.Application.Common.Models;
using MesaApi.Infrastructure.Data;

namespace MesaApi.Application.Features.Workflows.Queries.GetWorkflowById;

public class GetWorkflowByIdQueryHandler : IRequestHandler<GetWorkflowByIdQuery, Result<WorkflowDetailDto>>
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<GetWorkflowByIdQueryHandler> _logger;

    public GetWorkflowByIdQueryHandler(
        ApplicationDbContext context,
        ILogger<GetWorkflowByIdQueryHandler> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<Result<WorkflowDetailDto>> Handle(GetWorkflowByIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var workflow = await _context.Workflows
                .Include(w => w.Steps)
                .ThenInclude(s => s.Role)
                .FirstOrDefaultAsync(w => w.Id == request.Id, cancellationToken);

            if (workflow == null)
            {
                _logger.LogWarning("Workflow with ID {WorkflowId} not found", request.Id);
                return Result<WorkflowDetailDto>.Failure($"Workflow with ID {request.Id} not found");
            }

            var steps = workflow.Steps
                .OrderBy(s => s.Order)
                .Select(s => new WorkflowStepDto(
                    Id: s.Id,
                    StepName: s.StepName,
                    StepType: s.StepType,
                    Order: s.Order,
                    RoleId: s.RoleId,
                    RoleName: s.Role?.Name
                ))
                .ToList();

            var workflowDto = new WorkflowDetailDto(
                Id: workflow.Id,
                Name: workflow.Name,
                Description: workflow.Description,
                Category: workflow.Category,
                IsActive: workflow.IsActive,
                Steps: steps,
                CreatedAt: workflow.CreatedAt,
                CreatedBy: workflow.CreatedBy ?? "System",
                UpdatedAt: workflow.UpdatedAt,
                UpdatedBy: workflow.UpdatedBy
            );

            _logger.LogInformation("Retrieved workflow with ID: {WorkflowId}", request.Id);
            
            return Result<WorkflowDetailDto>.Success(workflowDto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving workflow with ID: {WorkflowId}", request.Id);
            return Result<WorkflowDetailDto>.Failure("An error occurred while retrieving the workflow");
        }
    }
}