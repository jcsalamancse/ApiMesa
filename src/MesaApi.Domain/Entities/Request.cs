using MesaApi.Domain.Enums;

namespace MesaApi.Domain.Entities;

public class Request : BaseEntity
{
    public string Description { get; set; } = string.Empty;
    public RequestStatus Status { get; set; } = RequestStatus.Pending;
    public RequestPriority Priority { get; set; } = RequestPriority.Medium;
    public string? Category { get; set; }
    public string? SubCategory { get; set; }
    public int RequesterId { get; set; }
    public int? AssignedToId { get; set; }
    public DateTime? DueDate { get; set; }
    public DateTime? CompletedAt { get; set; }
    
    // Navigation properties
    public User Requester { get; set; } = null!;
    public User? AssignedTo { get; set; }
    public ICollection<RequestStep> Steps { get; set; } = new List<RequestStep>();
    public ICollection<Comment> Comments { get; set; } = new List<Comment>();
    public ICollection<RequestData> RequestData { get; set; } = new List<RequestData>();
    public ICollection<Attachment> Attachments { get; set; } = new List<Attachment>();
}
pub
lic class RequestStep : BaseEntity
{
    public int RequestId { get; set; }
    public string StepName { get; set; } = string.Empty;
    public string? StepType { get; set; }
    public int Order { get; set; }
    public StepStatus Status { get; set; } = StepStatus.Pending;
    public int? AssignedToId { get; set; }
    public int? RoleId { get; set; }
    public DateTime? CompletedAt { get; set; }
    public string? Notes { get; set; }
    
    // Navigation properties
    public Request Request { get; set; } = null!;
    public User? AssignedTo { get; set; }
    public Role? Role { get; set; }
}

public class Comment : BaseEntity
{
    public int RequestId { get; set; }
    public int UserId { get; set; }
    public string Content { get; set; } = string.Empty;
    public bool IsInternal { get; set; } = false;
    
    // Navigation properties
    public Request Request { get; set; } = null!;
    public User User { get; set; } = null!;
}

public class RequestData : BaseEntity
{
    public int RequestId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Value { get; set; } = string.Empty;
    public string? DataType { get; set; }
    
    // Navigation properties
    public Request Request { get; set; } = null!;
}

public class Attachment : BaseEntity
{
    public int RequestId { get; set; }
    public string FileName { get; set; } = string.Empty;
    public string FilePath { get; set; } = string.Empty;
    public string ContentType { get; set; } = string.Empty;
    public long FileSize { get; set; }
    
    // Navigation properties
    public Request Request { get; set; } = null!;
}

public class Role : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public bool IsActive { get; set; } = true;
    
    // Navigation properties
    public ICollection<RequestStep> RequestSteps { get; set; } = new List<RequestStep>();
}

public class UserSession : BaseEntity
{
    public int UserId { get; set; }
    public string SessionId { get; set; } = string.Empty;
    public string Login { get; set; } = string.Empty;
    public DateTime ExpiresAt { get; set; }
    public bool IsActive { get; set; } = true;
    
    // Navigation properties
    public User User { get; set; } = null!;
}