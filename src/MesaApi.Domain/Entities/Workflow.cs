namespace MesaApi.Domain.Entities;

public class Workflow : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;
    
    // Navigation properties
    public ICollection<WorkflowStep> Steps { get; set; } = new List<WorkflowStep>();
}

public class WorkflowStep : BaseEntity
{
    public int WorkflowId { get; set; }
    public string StepName { get; set; } = string.Empty;
    public string? StepType { get; set; }
    public int Order { get; set; }
    public int? RoleId { get; set; }
    
    // Navigation properties
    public Workflow Workflow { get; set; } = null!;
    public Role? Role { get; set; }
}