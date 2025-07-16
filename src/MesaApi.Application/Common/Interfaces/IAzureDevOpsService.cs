using MesaApi.Application.Common.Models;

namespace MesaApi.Application.Common.Interfaces;

/// <summary>
/// Servicio para integración con Azure DevOps
/// </summary>
public interface IAzureDevOpsService
{
    /// <summary>
    /// Obtiene tareas asignadas al usuario basadas en su perfil
    /// </summary>
    Task<Result<List<DevOpsWorkItem>>> GetTasksByUserProfileAsync(string userId, UserProfile profile);
    
    /// <summary>
    /// Crea un work item en Azure DevOps desde una solicitud escalada
    /// </summary>
    Task<Result<DevOpsWorkItem>> CreateWorkItemFromRequestAsync(int requestId, WorkItemType type, WorkItemData data);
    
    /// <summary>
    /// Sincroniza el estado entre la solicitud y el work item
    /// </summary>
    Task<Result<bool>> SyncRequestStatusAsync(int requestId, int workItemId);
    
    /// <summary>
    /// Obtiene métricas del equipo de desarrollo
    /// </summary>
    Task<Result<TeamMetrics>> GetTeamMetricsAsync(string teamId, DateTime fromDate, DateTime toDate);
    
    /// <summary>
    /// Obtiene información de sprints activos
    /// </summary>
    Task<Result<List<SprintInfo>>> GetActiveSprintsAsync(string projectId);
    
    /// <summary>
    /// Vincula commits con solicitudes de soporte
    /// </summary>
    Task<Result<bool>> LinkCommitToRequestAsync(int requestId, string commitId, string repositoryId);
    
    /// <summary>
    /// Obtiene el estado de pipelines relacionados con una solicitud
    /// </summary>
    Task<Result<List<PipelineStatus>>> GetRelatedPipelinesAsync(int requestId);
}

public class DevOpsWorkItem
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public string State { get; set; } = string.Empty;
    public string AssignedTo { get; set; } = string.Empty;
    public string Priority { get; set; } = string.Empty;
    public DateTime CreatedDate { get; set; }
    public DateTime? CompletedDate { get; set; }
    public string Description { get; set; } = string.Empty;
    public List<string> Tags { get; set; } = new();
    public int? RelatedRequestId { get; set; }
}

public class WorkItemData
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Priority { get; set; } = string.Empty;
    public string AssignedTo { get; set; } = string.Empty;
    public string AreaPath { get; set; } = string.Empty;
    public string IterationPath { get; set; } = string.Empty;
    public Dictionary<string, object> CustomFields { get; set; } = new();
}

public enum WorkItemType
{
    Bug,
    Task,
    UserStory,
    Feature,
    Epic
}

public class UserProfile
{
    public string UserId { get; set; } = string.Empty;
    public List<string> Skills { get; set; } = new();
    public List<string> Teams { get; set; } = new();
    public string Role { get; set; } = string.Empty;
    public int CurrentWorkload { get; set; }
    public List<string> PreferredWorkItemTypes { get; set; } = new();
}

public class TeamMetrics
{
    public string TeamId { get; set; } = string.Empty;
    public int CompletedWorkItems { get; set; }
    public int ActiveWorkItems { get; set; }
    public double AverageCompletionTime { get; set; }
    public double VelocityPoints { get; set; }
    public List<TeamMemberMetric> MemberMetrics { get; set; } = new();
}

public class TeamMemberMetric
{
    public string UserId { get; set; } = string.Empty;
    public string DisplayName { get; set; } = string.Empty;
    public int CompletedItems { get; set; }
    public int ActiveItems { get; set; }
    public double AverageCompletionTime { get; set; }
}

public class SprintInfo
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string State { get; set; } = string.Empty;
    public int PlannedCapacity { get; set; }
    public int CompletedWork { get; set; }
    public int RemainingWork { get; set; }
}

public class PipelineStatus
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string Result { get; set; } = string.Empty;
    public DateTime? StartTime { get; set; }
    public DateTime? FinishTime { get; set; }
    public string Environment { get; set; } = string.Empty;
    public string Branch { get; set; } = string.Empty;
}