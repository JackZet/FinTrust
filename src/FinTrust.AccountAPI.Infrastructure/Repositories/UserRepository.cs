using FinTrust.AccountAPI.Domain.Entities;
using FinTrust.AccountAPI.Infrastructure.Repositories.Interfaces;

namespace FinTrust.AccountAPI.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    // dummy in-memory users
    private readonly List<User> _users =
    [
        new()
        {
            Id = Guid.NewGuid(),
            UserOid = "user-1"
        },
        new()
        {
        Id = Guid.NewGuid(),
        UserOid = "user-2"
        }
    ];
    
    public User? Get(string userOid)
    {
        return _users.FirstOrDefault(u => u.UserOid == userOid);
    }
}