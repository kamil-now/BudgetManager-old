namespace BudgetManager.Application.Features.BudgetManagement;

using AutoMapper;

public class CurrencyExchangeRequestHandler : BudgetRequestHandler<CurrencyExchangeRequest, CurrencyExchangeDto>
{
  public CurrencyExchangeRequestHandler(IUserBudgetRepository repo, IMapper map)
   : base(repo, map)
  {
  }

  public override CurrencyExchangeDto Get(CurrencyExchangeRequest request, Budget budget)
  {
    var currencyExchange = budget.Operations.First(x => x.Id == request.CurrencyExchangeId) as CurrencyExchange;
    if (currencyExchange is null)
    {
      throw new Exception();
    }
    return _mapper.Map<CurrencyExchangeDto>(currencyExchange)
     with
    {
      Type = MoneyOperationType.CurrencyExchange,
      AccountName = budget.Accounts.First(x => x.Id == currencyExchange.AccountId).Name,
    };
  }
}
