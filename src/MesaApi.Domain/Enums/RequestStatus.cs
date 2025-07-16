namespace MesaApi.Domain.Enums;

public enum RequestStatus
{
    Pending = 1,
    InProgress = 2,
    OnHold = 3,
    Completed = 4,
    Cancelled = 5,
    Rejected = 6
}

public enum RequestPriority
{
    Low = 1,
    Medium = 2,
    High = 3,
    Critical = 4
}

public enum StepStatus
{
    Pending = 1,
    InProgress = 2,
    Completed = 3,
    Skipped = 4
}