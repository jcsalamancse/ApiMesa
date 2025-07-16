using MediatR;
using MesaApi.Application.Common.Models;

namespace MesaApi.Application.Features.Comments.Commands.AddComment;

public record AddCommentCommand(
    int RequestId,
    string Content,
    bool IsInternal = false
) : IRequest<Result<CommentResponse>>;

public record CommentResponse(
    int Id,
    int RequestId,
    string Content,
    int UserId,
    string UserName,
    DateTime CreatedAt,
    bool IsInternal
);