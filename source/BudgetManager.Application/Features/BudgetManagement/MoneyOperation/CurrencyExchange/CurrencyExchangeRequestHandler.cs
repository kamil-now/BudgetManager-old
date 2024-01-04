namespace BudgetManager.Application.Features.BudgetManagement;

using AutoMapper;

public class CurrencyExchangeRequestHandler(IUserBudgetRepository repo, IMapper map)
  : BudgetRequestHandler<CurrencyExchangeRequest, CurrencyExchangeDto>(repo, map)
{
  public override CurrencyExchangeDto Get(CurrencyExchangeRequest request, Budget budget)
  {
    if (budget.Operations.First(x => x.Id == request.CurrencyExchangeId) is not CurrencyExchange currencyExchange)
    {
      throw new Exception();
    }
    return _mapper.Map<CurrencyExchangeDto>(currencyExchange) with
    {
      Type = MoneyOperationType.CurrencyExchange,
      AccountName = budget.Accounts.First(x => x.Id == currencyExchange.AccountId).Name,
    };
  }
}
