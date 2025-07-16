namespace MesaApi.Domain.Entities;

public class User : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Login { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;
    public string? Phone { get; set; }
    public string? Department { get; set; }
    public string? Position { get; set; }
    
    // Navigation properties
    public ICollection<Request> Requests { get; set; } = new List<Request>();
    public ICollection<RequestStep> RequestSteps { get; set; } = new List<RequestStep>();
    public ICollection<Comment> Comments { get; set; } = new List<Comment>();
    public ICollection<UserSession> Sessions { get; set; } = new List<UserSession>();
}