namespace FinTrust.AccountAPI.Domain;

public record AuditEvent
{
    public Guid Id { get; init; }
    public DateTime TimeStamp { get; init; } = DateTime.UtcNow;
    public required string ActorOid { get; set; }
    public required IEnumerable<string> ActorRoles { get; set; }
    public required string Action { get; set; }
    public required string ResourceType { get; set; }
    public required string ResourceId { get; set; }
    public required string Outcome { get; set; }
    public required string Reason { get; set; }
}

public static class AuditOutcome
{
    public const string Allowed = "Allowed";
    public const string Denied = "Denied";
}

public static class AuditAction
{
   public const string Read = "Read";
}

public static class AuditResourceType
{
    public const string Account = "Account";
}