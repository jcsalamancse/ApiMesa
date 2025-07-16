using MesaApi.Application.Common.Models;

namespace MesaApi.Application.Common.Interfaces;

/// <summary>
/// Servicio para asignación inteligente de tareas basada en perfiles
/// </summary>
public interface ITaskAssignmentService
{
    /// <summary>
    /// Obtiene el técnico óptimo para asignar una solicitud
    /// </summary>
    Task<Result<UserAssignmentRecommendation>> GetOptimalAssigneeAsync(int requestId);
    
    /// <summary>
    /// Obtiene la carga de trabajo actual de un usuario
    /// </summary>
    Task<Result<UserWorkload>> GetUserWorkloadAsync(string userId);
    
    /// <summary>
    /// Obtiene métricas de rendimiento de un usuario
    /// </summary>
    Task<Result<UserPerformanceMetrics>> GetUserPerformanceAsync(string userId, DateTime fromDate, DateTime toDate);
    
    /// <summary>
    /// Balancea automáticamente la carga de trabajo del equipo
    /// </summary>
    Task<Result<List<TaskReassignment>>> BalanceTeamWorkloadAsync(string teamId);
    
    /// <summary>
    /// Obtiene vista personalizada de tareas para un usuario
    /// </summary>
    Task<Result<PersonalizedTaskView>> GetPersonalizedTaskViewAsync(string userId);
    
    /// <summary>
    /// Programa mantenimientos preventivos
    /// </summary>
    Task<Result<bool>> SchedulePreventiveMaintenanceAsync(PreventiveMaintenanceRequest request);
    
    /// <summary>
    /// Obtiene disponibilidad del equipo para planificación
    /// </summary>
    Task<Result<TeamAvailability>> GetTeamAvailabilityAsync(string teamId, DateTime fromDate, DateTime toDate);
}

public class UserAssignmentRecommendation
{
    public string UserId { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
    public double ConfidenceScore { get; set; }
    public List<string> Reasons { get; set; } = new();
    public UserWorkload CurrentWorkload { get; set; } = new();
    public List<string> RelevantSkills { get; set; } = new();
    public double HistoricalSuccessRate { get; set; }
}

public class UserWorkload
{
    public string UserId { get; set; } = string.Empty;
    public int ActiveTasks { get; set; }
    public int HighPriorityTasks { get; set; }
    public int OverdueTasks { get; set; }
    public double CapacityUtilization { get; set; }
    public DateTime NextAvailableSlot { get; set; }
    public List<TaskSummary> CurrentTasks { get; set; } = new();
}

public class TaskSummary
{
    public int RequestId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Priority { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public DateTime DueDate { get; set; }
    public double EstimatedHours { get; set; }
}

public class UserPerformanceMetrics
{
    public string UserId { get; set; } = string.Empty;
    public double AverageResolutionTime { get; set; }
    public double CustomerSatisfactionScore { get; set; }
    public int TotalResolvedRequests { get; set; }
    public double FirstCallResolutionRate { get; set; }
    public List<CategoryPerformance> CategoryPerformance { get; set; } = new();
    public List<SkillProficiency> SkillProficiencies { get; set; } = new();
}

public class CategoryPerformance
{
    public string Category { get; set; } = string.Empty;
    public int RequestsHandled { get; set; }
    public double AverageResolutionTime { get; set; }
    public double SuccessRate { get; set; }
}

public class SkillProficiency
{
    public string Skill { get; set; } = string.Empty;
    public double ProficiencyLevel { get; set; }
    public int RequestsHandled { get; set; }
    public DateTime LastUsed { get; set; }
}

public class TaskReassignment
{
    public int RequestId { get; set; }
    public string FromUserId { get; set; } = string.Empty;
    public string ToUserId { get; set; } = string.Empty;
    public string Reason { get; set; } = string.Empty;
    public double ImpactScore { get; set; }
}

public class PersonalizedTaskView
{
    public string UserId { get; set; } = string.Empty;
    public List<TaskSummary> MyTasks { get; set; } = new();
    public List<TaskSummary> UpcomingSLAs { get; set; } = new();
    public List<KnowledgeBaseArticle> RelevantKnowledge { get; set; } = new();
    public List<TeamUpdate> TeamUpdates { get; set; } = new();
    public UserWorkload CurrentWorkload { get; set; } = new();
}

public class KnowledgeBaseArticle
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Summary { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public double RelevanceScore { get; set; }
    public DateTime LastUpdated { get; set; }
}

public class TeamUpdate
{
    public string Type { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public DateTime Timestamp { get; set; }
    public string Source { get; set; } = string.Empty;
}

public class PreventiveMaintenanceRequest
{
    public string SystemId { get; set; } = string.Empty;
    public string MaintenanceType { get; set; } = string.Empty;
    public DateTime ScheduledDate { get; set; }
    public string AssignedTo { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public List<string> AffectedServices { get; set; } = new();
    public int EstimatedDurationMinutes { get; set; }
}

public class TeamAvailability
{
    public string TeamId { get; set; } = string.Empty;
    public DateTime FromDate { get; set; }
    public DateTime ToDate { get; set; }
    public List<UserAvailability> MemberAvailability { get; set; } = new();
    public double TeamCapacity { get; set; }
    public List<ScheduledMaintenance> ScheduledMaintenances { get; set; } = new();
}

public class UserAvailability
{
    public string UserId { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
    public List<AvailabilitySlot> AvailableSlots { get; set; } = new();
    public List<AvailabilitySlot> BusySlots { get; set; } = new();
    public double CapacityPercentage { get; set; }
}

public class AvailabilitySlot
{
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public string Type { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
}

public class ScheduledMaintenance
{
    public int Id { get; set; }
    public string SystemId { get; set; } = string.Empty;
    public DateTime ScheduledDate { get; set; }
    public int DurationMinutes { get; set; }
    public string AssignedTo { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
}