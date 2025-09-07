using FinTrust.AccountAPI.Domain;

namespace FinTrust.AccountAPI.Infrastructure.Repositories.Interfaces;

public interface IAuditEventRepository
{
    public AuditEvent? GetAsync(Guid id);
    public void CreateAsync(AuditEvent auditEvent);
}