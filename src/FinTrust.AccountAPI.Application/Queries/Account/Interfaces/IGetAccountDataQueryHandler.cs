namespace FinTrust.AccountAPI.Application.Queries.Account.Interfaces;

public interface IGetAccountDataQueryHandler
{
    public GetAccountDataResult<Domain.Entities.Account> Handle(GetAccountDataQuery query);
}
