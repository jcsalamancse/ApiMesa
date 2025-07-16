using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using MesaApi.Application.Common.Models;
using MesaApi.Domain.Entities;
using MesaApi.Domain.Interfaces;
using System.Security.Claims;

namespace MesaApi.Application.Features.Workflows.Commands.CreateWorkflow;

public class CreateWorkflowCommandHandler : IRequestHandler<CreateWorkflowCommand, Result<CreateWorkflowResponse>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<CreateWorkflowCommandHandler> _logger;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CreateWorkflowCommandHandler(
        IUnitOfWork unitOfWork,
        ILogger<CreateWorkflowCommandHandler> logger,
        IHttpContextAccessor httpContextAccessor)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<Result<CreateWorkflowResponse>> Handle(CreateWorkflowCommand request, CancellationToken cancellationToken)
    {
        try
        {
            // Get current user name for audit
            var userName = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.Name)?.Value ?? "System";

            // Begin transaction
            await _unitOfWork.BeginTransactionAsync(cancellationToken);

            // Create workflow
            var workflow = new Workflow
            {
                Name = request.Name,
                Description = request.Description,
                Category = request.Category,
                IsActive = request.IsActive,
                CreatedBy = userName
            };

            // Save workflow
            var createdWorkflow = await _unitOfWork.Workflows.AddAsync(workflow, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            // Create workflow steps
            var workflowSteps = new List<WorkflowStep>();
            foreach (var step in request.Steps)
            {
                var workflowStep = new WorkflowStep
                {
                    WorkflowId = createdWorkflow.Id,
                    StepName = step.StepName,
                    StepType = step.StepType,
                    Order = step.Order,
                    RoleId = step.RoleId,
                    CreatedBy = userName
                };

                var createdStep = await _unitOfWork.WorkflowSteps.AddAsync(workflowStep, cancellationToken);
                workflowSteps.Add(createdStep);
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            // Commit transaction
            await _unitOfWork.CommitTransactionAsync(cancellationToken);

            // Get role names for response
            var stepResponses = new List<WorkflowStepResponseDto>();
            foreach (var step in workflowSteps)
            {
                string? roleName = null;
                if (step.RoleId.HasValue)
                {
                    var role = await _unitOfWork.Roles.GetByIdAsync(step.RoleId.Value, cancellationToken);
                    roleName = role?.Name;
                }

                stepResponses.Add(new WorkflowStepResponseDto(
                    Id: step.Id,
                    StepName: step.StepName,
                    StepType: step.StepType,
                    Order: step.Order,
                    RoleId: step.RoleId,
                    RoleName: roleName
                ));
            }

            // Create response
            var response = new CreateWorkflowResponse(
                Id: createdWorkflow.Id,
                Name: createdWorkflow.Name,
                Description: createdWorkflow.Description,
                Category: createdWorkflow.Category,
                IsActive: createdWorkflow.IsActive,
                Steps: stepResponses,
                CreatedAt: createdWorkflow.CreatedAt,
                CreatedBy: createdWorkflow.CreatedBy ?? userName
            );

            _logger.LogInformation("Workflow created successfully with ID: {WorkflowId}", createdWorkflow.Id);
            
            return Result<CreateWorkflowResponse>.Success(response);
        }
        catch (Exception ex)
        {
            await _unitOfWork.RollbackTransactionAsync(cancellationToken);
            _logger.LogError(ex, "Error creating workflow");
            return Result<CreateWorkflowResponse>.Failure("An error occurred while creating the workflow");
        }
    }
}