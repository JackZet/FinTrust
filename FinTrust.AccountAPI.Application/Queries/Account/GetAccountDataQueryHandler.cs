using FinTrust.AccountAPI.Application.Queries.Account.Interfaces;
using FinTrust.AccountAPI.Domain;
using FinTrust.AccountAPI.Infrastructure.Repositories.Interfaces;
using Microsoft.Extensions.Logging;

namespace FinTrust.AccountAPI.Application.Queries.Account;

public record GetAccountDataQuery(string UserOid, string AccountId, List<string> UserRoles);


public class GetAccountDataQueryHandler(ILogger<IGetAccountDataQueryHandler> logger, IUserRepository userRepository, IAccountRepository accountRepository, IAuditEventRepository auditEventRepository) : IGetAccountDataQueryHandler
{
    public GetAccountDataResult<Domain.Entities.Account> Handle(GetAccountDataQuery query)
    {
        var user = userRepository.Get(query.UserOid);
        if (user is null)
        {
            logger.LogInformation("User {userOid} tried to access account {accountId} but could not find user", query.UserOid, query.AccountId);
            
            var reason = "User not found";
            LogAndSaveAuditEvent(query, AuditOutcome.Denied, reason);
            
            return GetAccountDataResult<Domain.Entities.Account>.Forbidden(reason);
        }

        var account = accountRepository.GetAsync(query.AccountId);
        
        if (account is null)
        {
            logger.LogInformation("User {userOid} tried to access account {accountId} but could not find account", query.UserOid, query.AccountId);
            
            var reason = "Account not found";
            LogAndSaveAuditEvent(query, AuditOutcome.Denied, reason);
            
            return GetAccountDataResult<Domain.Entities.Account>.NotFound(reason);
        }

        if (!account.UserCanReadAccount(user, query.UserRoles))
        {
            logger.LogInformation("User {userOid} tried to access account {accountId} but did not have sufficient privileges", query.UserOid, query.AccountId);
            
            var reason = "Insufficient privileges to access account";
            LogAndSaveAuditEvent(query, AuditOutcome.Denied, reason);
            
            return GetAccountDataResult<Domain.Entities.Account>.Forbidden(reason);
        }
        
        
        logger.LogInformation("User {userId} has successfully retrieved account data for account {accountId}", query.UserOid, query.AccountId);
        
        LogAndSaveAuditEvent(query, AuditOutcome.Allowed, "User has sufficient roles and is a member of the account");
        return GetAccountDataResult<Domain.Entities.Account>.Success(account);
    }

    private void LogAndSaveAuditEvent(GetAccountDataQuery query, string outcome, string reason)
    {
        var auditEvent = new AuditEvent
        {
            ActorOid = query.UserOid,
            ActorRoles = query.UserRoles,
            Action = AuditAction.Read,
            ResourceType = AuditResourceType.Account,
            ResourceId = query.AccountId,
            Outcome = outcome,
            Reason = reason
        };
        
        try
        {
            logger.LogInformation("{@auditEvent}", auditEvent);
            auditEventRepository.CreateAsync(auditEvent);
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error saving audit event: {}", auditEvent);
        }
    }
}


public class GetAccountDataResult<T>
{
    public bool IsSuccess { get; set; }
    public T? Data { get; set; }
    public string? ErrorMessage { get; set; }
    public GetAccountDataErrorType ErrorType { get; set; }

    public static GetAccountDataResult<T> Success(T data) => new()
    {
        IsSuccess = true,
        Data = data,
    };
    
    public static GetAccountDataResult<T> Forbidden(string message) => new()
    {
        IsSuccess = false,
        ErrorMessage = message,
        ErrorType = GetAccountDataErrorType.Forbidden,
    };
    
    public static GetAccountDataResult<T> NotFound(string message) => new()
    {
        IsSuccess = false,
        ErrorMessage = message,
        ErrorType = GetAccountDataErrorType.NotFound,
    };
}

public enum GetAccountDataErrorType
{
    Forbidden,
    NotFound
}
