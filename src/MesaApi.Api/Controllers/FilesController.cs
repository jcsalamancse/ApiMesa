using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MesaApi.Application.Common.Interfaces;

namespace MesaApi.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class FilesController : ControllerBase
{
    private readonly IFileStorageService _fileStorageService;
    private readonly ILogger<FilesController> _logger;

    public FilesController(
        IFileStorageService fileStorageService,
        ILogger<FilesController> logger)
    {
        _fileStorageService = fileStorageService;
        _logger = logger;
    }

    /// <summary>
    /// Get file by path
    /// </summary>
    /// <param name="*">File path segments</param>
    /// <returns>File content</returns>
    [HttpGet("{**filePath}")]
    public async Task<IActionResult> GetFile(string filePath)
    {
        try
        {
            var fileBytes = await _fileStorageService.GetFileAsync(filePath);
            
            // Determine content type
            var contentType = GetContentType(filePath);
            
            return File(fileBytes, contentType);
        }
        catch (FileNotFoundException)
        {
            return NotFound(new { message = "File not found" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving file: {FilePath}", filePath);
            return StatusCode(500, new { message = $"Error retrieving file: {ex.Message}" });
        }
    }

    /// <summary>
    /// Upload a file to a specific directory
    /// </summary>
    /// <param name="directory">Directory to upload to</param>
    /// <param name="file">File to upload</param>
    /// <returns>File URL</returns>
    [HttpPost("upload/{directory}")]
    public async Task<IActionResult> UploadFile(string directory, IFormFile file)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest(new { message = "No file was uploaded" });
            }

            // Validate file size
            var maxFileSize = 10 * 1024 * 1024; // 10MB
            if (file.Length > maxFileSize)
            {
                return BadRequest(new { message = "File size exceeds the limit (10MB)" });
            }

            // Validate file extension
            var allowedExtensions = new[] { ".pdf", ".doc", ".docx", ".xls", ".xlsx", ".ppt", ".pptx", ".txt", ".csv", ".jpg", ".jpeg", ".png", ".gif" };
            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
            if (!allowedExtensions.Contains(extension))
            {
                return BadRequest(new { message = "File type not allowed" });
            }

            // Save file
            var filePath = await _fileStorageService.SaveFileAsync(file, directory);
            var fileUrl = _fileStorageService.GetFileUrl(filePath);

            return Ok(new { filePath, fileUrl });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error uploading file to directory: {Directory}", directory);
            return StatusCode(500, new { message = $"Error uploading file: {ex.Message}" });
        }
    }

    /// <summary>
    /// Delete a file
    /// </summary>
    /// <param name="filePath">Path to file</param>
    /// <returns>Success message</returns>
    [HttpDelete("{**filePath}")]
    [Authorize(Roles = "Administrador")]
    public async Task<IActionResult> DeleteFile(string filePath)
    {
        try
        {
            await _fileStorageService.DeleteFileAsync(filePath);
            return Ok(new { message = "File deleted successfully" });
        }
        catch (FileNotFoundException)
        {
            return NotFound(new { message = "File not found" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting file: {FilePath}", filePath);
            return StatusCode(500, new { message = $"Error deleting file: {ex.Message}" });
        }
    }

    private string GetContentType(string filePath)
    {
        var extension = Path.GetExtension(filePath).ToLowerInvariant();
        
        return extension switch
        {
            ".pdf" => "application/pdf",
            ".doc" => "application/msword",
            ".docx" => "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
            ".xls" => "application/vnd.ms-excel",
            ".xlsx" => "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
            ".ppt" => "application/vnd.ms-powerpoint",
            ".pptx" => "application/vnd.openxmlformats-officedocument.presentationml.presentation",
            ".txt" => "text/plain",
            ".csv" => "text/csv",
            ".jpg" => "image/jpeg",
            ".jpeg" => "image/jpeg",
            ".png" => "image/png",
            ".gif" => "image/gif",
            _ => "application/octet-stream"
        };
    }
}