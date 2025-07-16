using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using MesaApi.Application.Common.Interfaces;

namespace MesaApi.Infrastructure.Services;

public class LocalFileStorageService : IFileStorageService
{
    private readonly string _baseDirectory;
    private readonly string _baseUrl;

    public LocalFileStorageService(IConfiguration configuration)
    {
        _baseDirectory = configuration["FileStorage:BaseDirectory"] ?? "uploads";
        _baseUrl = configuration["FileStorage:BaseUrl"] ?? "/api/files";
        
        // Ensure base directory exists
        if (!Directory.Exists(_baseDirectory))
        {
            Directory.CreateDirectory(_baseDirectory);
        }
    }

    public async Task<string> SaveFileAsync(IFormFile file, string directory, CancellationToken cancellationToken = default)
    {
        // Create directory if it doesn't exist
        var fullDirectory = Path.Combine(_baseDirectory, directory);
        if (!Directory.Exists(fullDirectory))
        {
            Directory.CreateDirectory(fullDirectory);
        }

        // Generate unique filename
        var fileExtension = Path.GetExtension(file.FileName);
        var uniqueFileName = $"{Guid.NewGuid()}{fileExtension}";
        var filePath = Path.Combine(fullDirectory, uniqueFileName);

        // Save file
        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(stream, cancellationToken);
        }

        // Return relative path
        return Path.Combine(directory, uniqueFileName);
    }

    public async Task<byte[]> GetFileAsync(string filePath, CancellationToken cancellationToken = default)
    {
        var fullPath = Path.Combine(_baseDirectory, filePath);
        
        if (!File.Exists(fullPath))
        {
            throw new FileNotFoundException("File not found", fullPath);
        }

        return await File.ReadAllBytesAsync(fullPath, cancellationToken);
    }

    public Task DeleteFileAsync(string filePath, CancellationToken cancellationToken = default)
    {
        var fullPath = Path.Combine(_baseDirectory, filePath);
        
        if (File.Exists(fullPath))
        {
            File.Delete(fullPath);
        }

        return Task.CompletedTask;
    }

    public string GetFileUrl(string filePath)
    {
        return $"{_baseUrl}/{filePath}";
    }
}