using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using MesaApi.Application.Common.Models;
using MesaApi.Domain.Entities;
using MesaApi.Domain.Enums;
using MesaApi.Domain.Interfaces;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace MesaApi.Application.Features.Requests.Commands.CreateRequest;

public class CreateRequestCommandHandler : IRequestHandler<CreateRequestCommand, Result<CreateRequestResponse>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<CreateRequestCommandHandler> _logger;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CreateRequestCommandHandler(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        ILogger<CreateRequestCommandHandler> logger,
        IHttpContextAccessor httpContextAccessor)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<Result<CreateRequestResponse>> Handle(CreateRequestCommand request, CancellationToken cancellationToken)
    {
        try
        {
            // Get current user ID from claims
            var userIdClaim = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            
            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out var userId))
            {
                return Result<CreateRequestResponse>.Failure("User not authenticated or invalid user ID");
            }

            // Get user
            var user = await _unitOfWork.Users.GetByIdAsync(userId, cancellationToken);
            if (user == null)
            {
                return Result<CreateRequestResponse>.Failure("User not found");
            }

            // Create request
            var newRequest = new Request
            {
                Description = request.Description,
                Status = RequestStatus.Pending,
                Priority = request.Priority,
                Category = request.Category,
                SubCategory = request.SubCategory,
                RequesterId = userId,
                DueDate = request.DueDate,
                CreatedBy = user.Name
            };

            // Begin transaction
            await _unitOfWork.BeginTransactionAsync(cancellationToken);

            // Save request
            var createdRequest = await _unitOfWork.Requests.AddAsync(newRequest, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            // Add request data if provided
            if (request.RequestData != null && request.RequestData.Any())
            {
                foreach (var dataItem in request.RequestData)
                {
                    var requestData = new RequestData
                    {
                        RequestId = createdRequest.Id,
                        Name = dataItem.Name,
                        Value = dataItem.Value,
                        DataType = dataItem.DataType,
                        CreatedBy = user.Name
                    };

                    await _unitOfWork.SaveChangesAsync(cancellationToken);
                }
            }

            // Create initial step
            var initialStep = new RequestStep
            {
                RequestId = createdRequest.Id,
                StepName = "Solicitud Creada",
                StepType = "Inicio",
                Order = 1,
                Status = StepStatus.Completed,
                CompletedAt = DateTime.UtcNow,
                CreatedBy = user.Name
            };

            await _unitOfWork.RequestSteps.AddAsync(initialStep, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            // Commit transaction
            await _unitOfWork.CommitTransactionAsync(cancellationToken);

            // Get request with details for response
            var requestWithDetails = await _unitOfWork.Requests.GetWithDetailsAsync(createdRequest.Id, cancellationToken);

            // Map to response
            var response = new CreateRequestResponse(
                Id: requestWithDetails!.Id,
                Description: requestWithDetails.Description,
                Status: requestWithDetails.Status.ToString(),
                Priority: requestWithDetails.Priority.ToString(),
                Category: requestWithDetails.Category,
                SubCategory: requestWithDetails.SubCategory,
                RequesterId: requestWithDetails.RequesterId,
                RequesterName: requestWithDetails.Requester.Name,
                DueDate: requestWithDetails.DueDate,
                CreatedAt: requestWithDetails.CreatedAt,
                RequestData: requestWithDetails.RequestData.Select(rd => new RequestDataDto(rd.Name, rd.Value, rd.DataType)).ToList()
            );

            _logger.LogInformation("Request created successfully with ID: {RequestId}", createdRequest.Id);
            
            return Result<CreateRequestResponse>.Success(response);
        }
        catch (Exception ex)
        {
            await _unitOfWork.RollbackTransactionAsync(cancellationToken);
            _logger.LogError(ex, "Error creating request");
            return Result<CreateRequestResponse>.Failure("An error occurred while creating the request");
        }
    }
}