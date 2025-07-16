using AutoMapper;
using MesaApi.Application.Features.Users.Commands.CreateUser;
using MesaApi.Application.Features.Auth.Commands.Login;
using MesaApi.Domain.Entities;

namespace MesaApi.Application.Common.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // User mappings
        CreateMap<User, CreateUserResponse>();
        CreateMap<User, UserInfo>();
        
        // Request mappings
        CreateMap<Request, RequestDto>()
            .ForMember(dest => dest.RequesterName, opt => opt.MapFrom(src => src.Requester.Name))
            .ForMember(dest => dest.AssignedToName, opt => opt.MapFrom(src => src.AssignedTo != null ? src.AssignedTo.Name : null));
            
        CreateMap<Comment, CommentDto>()
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.Name));
            
        CreateMap<RequestStep, RequestStepDto>()
            .ForMember(dest => dest.AssignedToName, opt => opt.MapFrom(src => src.AssignedTo != null ? src.AssignedTo.Name : null))
            .ForMember(dest => dest.RoleName, opt => opt.MapFrom(src => src.Role != null ? src.Role.Name : null));
    }
}

// DTOs
public record RequestDto(
    int Id,
    string Description,
    string Status,
    string Priority,
    string? Category,
    string? SubCategory,
    string RequesterName,
    string? AssignedToName,
    DateTime? DueDate,
    DateTime CreatedAt,
    DateTime? CompletedAt
);

public record CommentDto(
    int Id,
    string Content,
    string UserName,
    DateTime CreatedAt,
    bool IsInternal
);

public record RequestStepDto(
    int Id,
    string StepName,
    string? StepType,
    int Order,
    string Status,
    string? AssignedToName,
    string? RoleName,
    DateTime? CompletedAt,
    string? Notes
);