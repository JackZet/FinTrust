using FinTrust.AccountAPI.Domain;
using FinTrust.AccountAPI.Infrastructure.Repositories.Interfaces;

namespace FinTrust.AccountAPI.Infrastructure.Repositories;

public class AuditEventRepository : IAuditEventRepository
{
    private readonly List<AuditEvent> _auditEvents = [];
    
    public AuditEvent? GetAsync(Guid id)
    {
        return _auditEvents.FirstOrDefault(e => e.Id == id);
    }

    public void CreateAsync(AuditEvent auditEvent)
    {
        _auditEvents.Add(auditEvent);
    }
}