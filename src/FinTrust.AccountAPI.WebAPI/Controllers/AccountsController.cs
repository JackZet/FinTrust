using System.Security.Claims;
using FinTrust.AccountAPI.Application.Queries.Account;
using FinTrust.AccountAPI.Application.Queries.Account.Interfaces;
using FinTrust.AccountAPI.Domain;
using FinTrust.AccountAPI.WebAPI.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace FinTrust.AccountAPI.WebAPI.Controllers;

[Authorize(Policy = Policies.AccountAccess)]
[ApiController]
[Route("api/accounts")]
public class AccountsController(ILogger<AccountsController> logger): ControllerBase
{
    [HttpGet("{accountId}")]
    public IActionResult GetAccountData(string accountId, [FromServices] IGetAccountDataQueryHandler handler)
    {
        //Validate JWT token and extract user claims
        var userOid = User.FindFirst("oid")?.Value;
        if (userOid is null)
        {
            logger.LogInformation("Forbidden. User oid is null");
            return Forbid();
        }
        
        var roles = User.FindAll(ClaimTypes.Role).Select(r => r.Value).ToList();
        logger.LogInformation("User {userOid} attempting to access account {accountId}", userOid, accountId);


        //Check if user has required role and access to the account
        var query = new GetAccountDataQuery(userOid, accountId, roles);
        var result = handler.Handle(query);

        //Return account data or 403 Forbidden
        if (!result.IsSuccess)
        {
            logger.LogInformation("User {userOid} failed to get account data for account {accountId}", userOid, accountId);
            return result.ErrorType switch
            {
                GetAccountDataErrorType.Forbidden => Forbid(),
                GetAccountDataErrorType.NotFound => NotFound(),
                _ => StatusCode(500, "An error occurred while processing your request")
            };
        }
        
        return Ok(new AccountDto
        {
            AccountId = result.Data!.AccountId,
            Balance = result.Data.Balance
        });
    }
}