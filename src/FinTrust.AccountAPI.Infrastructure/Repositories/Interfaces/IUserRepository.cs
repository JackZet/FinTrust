using FinTrust.AccountAPI.Domain.Entities;

namespace FinTrust.AccountAPI.Infrastructure.Repositories.Interfaces;

public interface IUserRepository
{
    public User? Get(string userOid);
}