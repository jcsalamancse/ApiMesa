using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MesaApi.Application.Common.Models;
using MesaApi.Domain.Entities;
using MesaApi.Domain.Enums;
using MesaApi.Domain.Interfaces;
using MesaApi.Infrastructure.Data;
using System.Security.Claims;

namespace MesaApi.Application.Features.Workflows.Commands.ApplyWorkflow;

public class ApplyWorkflowCommandHandler : IRequestHandler<ApplyWorkflowCommand, Result<ApplyWorkflowResponse>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ApplicationDbContext _context;
    private readonly ILogger<ApplyWorkflowCommandHandler> _logger;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public ApplyWorkflowCommandHandler(
        IUnitOfWork unitOfWork,
        ApplicationDbContext context,
        ILogger<ApplyWorkflowCommandHandler> logger,
        IHttpContextAccessor httpContextAccessor)
    {
        _unitOfWork = unitOfWork;
        _context = context;
        _logger = logger;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<Result<ApplyWorkflowResponse>> Handle(ApplyWorkflowCommand request, CancellationToken cancellationToken)
    {
        try
        {
            // Get current user ID and name for audit
            var userIdClaim = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var userName = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.Name)?.Value ?? "System";
            
            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out var userId))
            {
                return Result<ApplyWorkflowResponse>.Failure("User not authenticated or invalid user ID");
            }

            // Check if request exists
            var request_ = await _unitOfWork.Requests.GetByIdAsync(request.RequestId, cancellationToken);
            if (request_ == null)
            {
                return Result<ApplyWorkflowResponse>.Failure($"Request with ID {request.RequestId} not found");
            }

            // Check if workflow exists
            var workflow = await _context.Workflows
                .Include(w => w.Steps)
                .ThenInclude(s => s.Role)
                .FirstOrDefaultAsync(w => w.Id == request.WorkflowId, cancellationToken);
                
            if (workflow == null)
            {
                return Result<ApplyWorkflowResponse>.Failure($"Workflow with ID {request.WorkflowId} not found");
            }

            // Check if workflow is active
            if (!workflow.IsActive)
            {
                return Result<ApplyWorkflowResponse>.Failure("Cannot apply inactive workflow");
            }

            // Begin transaction
            await _unitOfWork.BeginTransactionAsync(cancellationToken);

            // Clear existing steps if any
            var existingSteps = await _context.RequestSteps
                .Where(s => s.RequestId == request.RequestId)
                .ToListAsync(cancellationToken);
                
            foreach (var step in existingSteps)
            {
                step.IsDeleted = true;
                step.UpdatedAt = DateTime.UtcNow;
                step.UpdatedBy = userName;
            }
            
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            // Create new steps from workflow
            var appliedSteps = new List<RequestStep>();
            foreach (var workflowStep in workflow.Steps.OrderBy(s => s.Order))
            {
                var requestStep = new RequestStep
                {
                    RequestId = request.RequestId,
                    StepName = workflowStep.StepName,
                    StepType = workflowStep.StepType,
                    Order = workflowStep.Order,
                    Status = StepStatus.Pending,
                    RoleId = workflowStep.RoleId,
                    CreatedBy = userName
                };

                var createdStep = await _unitOfWork.RequestSteps.AddAsync(requestStep, cancellationToken);
                appliedSteps.Add(createdStep);
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            // Update request status to InProgress if it was Pending
            if (request_.Status == RequestStatus.Pending)
            {
                request_.Status = RequestStatus.InProgress;
                request_.UpdatedAt = DateTime.UtcNow;
                request_.UpdatedBy = userName;
                
                await _unitOfWork.Requests.UpdateAsync(request_, cancellationToken);
                await _unitOfWork.SaveChangesAsync(cancellationToken);
            }

            // Add comment about workflow application
            var comment = new Comment
            {
                RequestId = request.RequestId,
                UserId = userId,
                Content = $"Applied workflow: {workflow.Name}",
                IsInternal = true,
                CreatedBy = userName
            };

            await _unitOfWork.Comments.AddAsync(comment, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            // Commit transaction
            await _unitOfWork.CommitTransactionAsync(cancellationToken);

            // Create response
            var appliedStepDtos = appliedSteps.Select(s => new AppliedStepDto(
                Id: s.Id,
                StepName: s.StepName,
                StepType: s.StepType,
                Order: s.Order,
                Status: s.Status.ToString(),
                RoleId: s.RoleId,
                RoleName: workflow.Steps.FirstOrDefault(ws => ws.RoleId == s.RoleId)?.Role?.Name
            )).ToList();

            var response = new ApplyWorkflowResponse(
                RequestId: request.RequestId,
                WorkflowId: workflow.Id,
                WorkflowName: workflow.Name,
                AppliedSteps: appliedStepDtos,
                AppliedAt: DateTime.UtcNow,
                AppliedBy: userName
            );

            _logger.LogInformation("Workflow {WorkflowId} applied to request {RequestId} by user {UserId}", request.WorkflowId, request.RequestId, userId);
            
            return Result<ApplyWorkflowResponse>.Success(response);
        }
        catch (Exception ex)
        {
            await _unitOfWork.RollbackTransactionAsync(cancellationToken);
            _logger.LogError(ex, "Error applying workflow {WorkflowId} to request {RequestId}", request.WorkflowId, request.RequestId);
            return Result<ApplyWorkflowResponse>.Failure("An error occurred while applying the workflow");
        }
    }
}