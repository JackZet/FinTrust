using FinTrust.AccountAPI.Domain.Entities;
using FinTrust.AccountAPI.Infrastructure.Repositories.Interfaces;

namespace FinTrust.AccountAPI.Infrastructure.Repositories;

public class AccountRepository : IAccountRepository
{
    // dummy in-memory accounts
    private readonly List<Account> _accounts =
    [
        new()
        {
            AccountId = "account-1",
            Members =
            [
                new User
                {
                    Id = Guid.NewGuid(),
                    UserOid = "user-1"
                }
            ],
            Balance = 100
        },
        new()
        {
        AccountId = "account-2",
        Members = [],
        Balance = 51231989
        }
    ];
    
    public Account? GetAsync(string accountId)
    {
        return _accounts.FirstOrDefault(x => x.AccountId == accountId);
    }
}
