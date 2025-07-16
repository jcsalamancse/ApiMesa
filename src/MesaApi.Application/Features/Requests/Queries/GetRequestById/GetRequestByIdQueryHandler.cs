using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using MesaApi.Application.Common.Models;
using MesaApi.Domain.Interfaces;

namespace MesaApi.Application.Features.Requests.Queries.GetRequestById;

public class GetRequestByIdQueryHandler : IRequestHandler<GetRequestByIdQuery, Result<RequestDetailDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<GetRequestByIdQueryHandler> _logger;

    public GetRequestByIdQueryHandler(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        ILogger<GetRequestByIdQueryHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<Result<RequestDetailDto>> Handle(GetRequestByIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var requestEntity = await _unitOfWork.Requests.GetWithDetailsAsync(request.Id, cancellationToken);

            if (requestEntity == null)
            {
                _logger.LogWarning("Request with ID {RequestId} not found", request.Id);
                return Result<RequestDetailDto>.Failure($"Request with ID {request.Id} not found");
            }

            // Map to response DTO
            var response = new RequestDetailDto(
                Id: requestEntity.Id,
                Description: requestEntity.Description,
                Status: requestEntity.Status.ToString(),
                Priority: requestEntity.Priority.ToString(),
                Category: requestEntity.Category,
                SubCategory: requestEntity.SubCategory,
                RequesterId: requestEntity.RequesterId,
                RequesterName: requestEntity.Requester.Name,
                AssignedToId: requestEntity.AssignedToId,
                AssignedToName: requestEntity.AssignedTo?.Name,
                DueDate: requestEntity.DueDate,
                CreatedAt: requestEntity.CreatedAt,
                CompletedAt: requestEntity.CompletedAt,
                
                Steps: requestEntity.Steps
                    .OrderBy(s => s.Order)
                    .Select(s => new RequestStepDto(
                        Id: s.Id,
                        StepName: s.StepName,
                        StepType: s.StepType,
                        Order: s.Order,
                        Status: s.Status.ToString(),
                        AssignedToId: s.AssignedToId,
                        AssignedToName: s.AssignedTo?.Name,
                        RoleId: s.RoleId,
                        RoleName: s.Role?.Name,
                        CompletedAt: s.CompletedAt,
                        Notes: s.Notes
                    ))
                    .ToList(),
                
                Comments: requestEntity.Comments
                    .OrderByDescending(c => c.CreatedAt)
                    .Select(c => new CommentDto(
                        Id: c.Id,
                        Content: c.Content,
                        UserId: c.UserId,
                        UserName: c.User.Name,
                        CreatedAt: c.CreatedAt,
                        IsInternal: c.IsInternal
                    ))
                    .ToList(),
                
                RequestData: requestEntity.RequestData
                    .Select(rd => new RequestDataItemDto(
                        Id: rd.Id,
                        Name: rd.Name,
                        Value: rd.Value,
                        DataType: rd.DataType
                    ))
                    .ToList(),
                
                Attachments: requestEntity.Attachments
                    .OrderByDescending(a => a.CreatedAt)
                    .Select(a => new AttachmentDto(
                        Id: a.Id,
                        FileName: a.FileName,
                        FilePath: a.FilePath,
                        ContentType: a.ContentType,
                        FileSize: a.FileSize,
                        CreatedAt: a.CreatedAt
                    ))
                    .ToList()
            );

            _logger.LogInformation("Retrieved request with ID: {RequestId}", request.Id);
            
            return Result<RequestDetailDto>.Success(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving request with ID: {RequestId}", request.Id);
            return Result<RequestDetailDto>.Failure("An error occurred while retrieving the request");
        }
    }
}