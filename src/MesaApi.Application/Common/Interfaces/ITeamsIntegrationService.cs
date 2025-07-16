using MesaApi.Application.Common.Models;

namespace MesaApi.Application.Common.Interfaces;

/// <summary>
/// Servicio para integración con Microsoft Teams
/// </summary>
public interface ITeamsIntegrationService
{
    /// <summary>
    /// Envía notificación a un canal de Teams
    /// </summary>
    Task<Result<bool>> SendChannelNotificationAsync(string channelId, TeamsMessage message);
    
    /// <summary>
    /// Crea un canal automático para una solicitud crítica
    /// </summary>
    Task<Result<TeamsChannel>> CreateIncidentChannelAsync(int requestId, List<string> stakeholders);
    
    /// <summary>
    /// Envía mensaje directo a un usuario
    /// </summary>
    Task<Result<bool>> SendDirectMessageAsync(string userId, TeamsMessage message);
    
    /// <summary>
    /// Crea una tarjeta adaptativa para aprobación de documentos
    /// </summary>
    Task<Result<bool>> SendApprovalCardAsync(string userId, ApprovalRequest approval);
    
    /// <summary>
    /// Obtiene información del usuario de Teams
    /// </summary>
    Task<Result<TeamsUser>> GetUserInfoAsync(string userId);
    
    /// <summary>
    /// Programa una reunión para revisión de solicitud
    /// </summary>
    Task<Result<TeamsMeeting>> ScheduleReviewMeetingAsync(int requestId, List<string> attendees, DateTime scheduledTime);
    
    /// <summary>
    /// Actualiza el estado de una solicitud en el tab de Teams
    /// </summary>
    Task<Result<bool>> UpdateRequestStatusInTabAsync(int requestId, string status);
}

public class TeamsMessage
{
    public string Title { get; set; } = string.Empty;
    public string Text { get; set; } = string.Empty;
    public string Color { get; set; } = "0078D4"; // Azul por defecto
    public List<TeamsMessageAction> Actions { get; set; } = new();
    public Dictionary<string, object> AdditionalData { get; set; } = new();
}

public class TeamsMessageAction
{
    public string Type { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Url { get; set; } = string.Empty;
    public Dictionary<string, object> Data { get; set; } = new();
}

public class TeamsChannel
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string WebUrl { get; set; } = string.Empty;
    public List<string> Members { get; set; } = new();
    public DateTime CreatedAt { get; set; }
}

public class ApprovalRequest
{
    public int RequestId { get; set; }
    public string DocumentType { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string RequestedBy { get; set; } = string.Empty;
    public DateTime RequestedAt { get; set; }
    public string DocumentUrl { get; set; } = string.Empty;
    public List<ApprovalAction> Actions { get; set; } = new();
}

public class ApprovalAction
{
    public string ActionId { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Style { get; set; } = string.Empty; // "positive", "destructive", "default"
}

public class TeamsUser
{
    public string Id { get; set; } = string.Empty;
    public string DisplayName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string JobTitle { get; set; } = string.Empty;
    public string Department { get; set; } = string.Empty;
    public bool IsOnline { get; set; }
    public string Presence { get; set; } = string.Empty;
}

public class TeamsMeeting
{
    public string Id { get; set; } = string.Empty;
    public string Subject { get; set; } = string.Empty;
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public string JoinUrl { get; set; } = string.Empty;
    public List<string> Attendees { get; set; } = new();
    public string Organizer { get; set; } = string.Empty;
}