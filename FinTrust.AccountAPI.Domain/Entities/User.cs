namespace FinTrust.AccountAPI.Domain.Entities;

public class User
{
    public required Guid Id { get; set; }
    public required string UserOid { get; set; }
}