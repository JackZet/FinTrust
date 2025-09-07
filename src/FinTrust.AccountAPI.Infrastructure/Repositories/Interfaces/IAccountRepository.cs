using FinTrust.AccountAPI.Domain.Entities;

namespace FinTrust.AccountAPI.Infrastructure.Repositories.Interfaces;

public interface IAccountRepository
{
    public Account? GetAsync(string accountId);
}