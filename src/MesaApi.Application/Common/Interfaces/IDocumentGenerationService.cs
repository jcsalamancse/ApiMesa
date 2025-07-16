using MesaApi.Application.Common.Models;

namespace MesaApi.Application.Common.Interfaces;

/// <summary>
/// Servicio para generación automática de documentos (actas, pases a producción, etc.)
/// </summary>
public interface IDocumentGenerationService
{
    /// <summary>
    /// Genera un acta basada en una plantilla y datos de la solicitud
    /// </summary>
    Task<Result<byte[]>> GenerateActaAsync(int requestId, string templateName, Dictionary<string, object>? additionalData = null);
    
    /// <summary>
    /// Genera documento de pase a producción con formulario estandarizado
    /// </summary>
    Task<Result<byte[]>> GenerateProductionPassAsync(int requestId, ProductionPassData data);
    
    /// <summary>
    /// Crea una versión de documento con control de versiones
    /// </summary>
    Task<Result<DocumentVersion>> CreateVersionedDocumentAsync(string documentType, int requestId, object data);
    
    /// <summary>
    /// Obtiene plantillas disponibles por tipo
    /// </summary>
    Task<Result<List<DocumentTemplate>>> GetTemplatesByTypeAsync(string documentType);
    
    /// <summary>
    /// Valida formulario de pase a producción
    /// </summary>
    Task<Result<ValidationResult>> ValidateProductionPassFormAsync(ProductionPassData data);
}

public class ProductionPassData
{
    public string Environment { get; set; } = string.Empty;
    public DateTime ScheduledDate { get; set; }
    public string Description { get; set; } = string.Empty;
    public List<string> AffectedSystems { get; set; } = new();
    public string RollbackPlan { get; set; } = string.Empty;
    public List<ApprovalData> Approvals { get; set; } = new();
    public Dictionary<string, object> CustomFields { get; set; } = new();
}

public class ApprovalData
{
    public string ApproverRole { get; set; } = string.Empty;
    public string ApproverName { get; set; } = string.Empty;
    public DateTime? ApprovalDate { get; set; }
    public string Status { get; set; } = string.Empty;
    public string Comments { get; set; } = string.Empty;
}

public class DocumentVersion
{
    public int Id { get; set; }
    public string DocumentType { get; set; } = string.Empty;
    public int RequestId { get; set; }
    public int Version { get; set; }
    public string FilePath { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public string CreatedBy { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
}

public class DocumentTemplate
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public string TemplatePath { get; set; } = string.Empty;
    public List<string> RequiredFields { get; set; } = new();
    public bool IsActive { get; set; }
}

public class ValidationResult
{
    public bool IsValid { get; set; }
    public List<string> Errors { get; set; } = new();
    public List<string> Warnings { get; set; } = new();
}