namespace BudgetManager.Application.Features.BudgetManagement;

using AutoMapper;

public class CurrencyExchangesListRequestHandler : BudgetRequestHandler<BudgetRequest<CurrencyExchangeDto>, IEnumerable<CurrencyExchangeDto>>
{
  public CurrencyExchangesListRequestHandler(IUserBudgetRepository repo, IMapper map)
   : base(repo, map)
  {
  }

  public override IEnumerable<CurrencyExchangeDto> Get(BudgetRequest<CurrencyExchangeDto> request, Budget budget)
   => budget.Operations.Where(x => x is CurrencyExchange).Select(x => _mapper.Map<CurrencyExchangeDto>(x as CurrencyExchange));
}
