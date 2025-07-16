using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MesaApi.Application.Features.Requests.Commands.UploadAttachment;
using MesaApi.Application.Features.Requests.Queries.GetRequestAttachments;
using MesaApi.Application.Common.Interfaces;

namespace MesaApi.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class AttachmentsController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IFileStorageService _fileStorageService;

    public AttachmentsController(
        IMediator mediator,
        IFileStorageService fileStorageService)
    {
        _mediator = mediator;
        _fileStorageService = fileStorageService;
    }

    /// <summary>
    /// Upload attachment to request
    /// </summary>
    /// <param name="requestId">Request ID</param>
    /// <param name="file">File to upload</param>
    /// <param name="description">Optional description</param>
    /// <returns>Uploaded attachment information</returns>
    [HttpPost("{requestId}")]
    public async Task<IActionResult> UploadAttachment(
        int requestId,
        [FromForm] IFormFile file,
        [FromForm] string? description = null)
    {
        var command = new UploadAttachmentCommand(
            RequestId: requestId,
            File: file,
            Description: description
        );

        var result = await _mediator.Send(command);
        
        if (!result.IsSuccess)
        {
            return BadRequest(new { message = result.Error, errors = result.Errors });
        }

        return Ok(result.Data);
    }

    /// <summary>
    /// Get attachments for request
    /// </summary>
    /// <param name="requestId">Request ID</param>
    /// <returns>List of attachments</returns>
    [HttpGet("request/{requestId}")]
    public async Task<IActionResult> GetRequestAttachments(int requestId)
    {
        var query = new GetRequestAttachmentsQuery(requestId);
        var result = await _mediator.Send(query);
        
        if (!result.IsSuccess)
        {
            return BadRequest(new { message = result.Error });
        }

        return Ok(result.Data);
    }

    /// <summary>
    /// Download attachment
    /// </summary>
    /// <param name="id">Attachment ID</param>
    /// <returns>File</returns>
    [HttpGet("{id}/download")]
    public async Task<IActionResult> DownloadAttachment(int id)
    {
        try
        {
            // Get attachment from database
            var query = new GetAttachmentByIdQuery(id);
            var result = await _mediator.Send(query);
            
            if (!result.IsSuccess)
            {
                return NotFound(new { message = result.Error });
            }

            // Get file from storage
            var fileBytes = await _fileStorageService.GetFileAsync(result.Data.FilePath);
            
            return File(fileBytes, result.Data.ContentType, result.Data.FileName);
        }
        catch (FileNotFoundException)
        {
            return NotFound(new { message = "File not found" });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = $"Error downloading attachment: {ex.Message}" });
        }
    }

    /// <summary>
    /// Delete attachment
    /// </summary>
    /// <param name="id">Attachment ID</param>
    /// <returns>Success message</returns>
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAttachment(int id)
    {
        try
        {
            var command = new DeleteAttachmentCommand(id);
            var result = await _mediator.Send(command);
            
            if (!result.IsSuccess)
            {
                return BadRequest(new { message = result.Error });
            }

            return Ok(new { message = "Attachment deleted successfully" });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = $"Error deleting attachment: {ex.Message}" });
        }
    }
}

// These record classes are placeholders for the actual implementations
// In a real project, they would be defined in their own files
public record GetAttachmentByIdQuery(int Id) : IRequest<Result<AttachmentDetailDto>>;
public record AttachmentDetailDto(int Id, string FileName, string FilePath, string ContentType);
public record DeleteAttachmentCommand(int Id) : IRequest<Result>;
public record Result { public bool IsSuccess { get; init; } public string? Error { get; init; } }
public record Result<T> { public bool IsSuccess { get; init; } public T? Data { get; init; } public string? Error { get; init; } }