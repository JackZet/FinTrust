namespace FinTrust.AccountAPI.Domain.Entities;

public class Account
{
    public Guid Id { get; init; }
    
    public required string AccountId {get; set; }
    public required IEnumerable<User> Members { get; set; }
    
    public required int Balance { get; set; }

    public bool UserCanReadAccount(User user, List<string> roles)
    {
        // admins can access all accounts
        if (roles.Contains(Roles.AccountAdmin))
        {
            return true;
        }

        // if the user has the AccountViewer role, and it is a member of the account - then the user can access the account 
        return roles.Contains(Roles.AccountViewer) && Members.Any(a => a.UserOid == user.UserOid);
    }
}