namespace BudgetManager.Application.Features.BudgetManagement;

using AutoMapper;

public class CurrencyExchangesListRequestHandler(IUserBudgetRepository repo, IMapper map) 
  : BudgetRequestHandler<BudgetRequest<CurrencyExchangeDto>, IEnumerable<CurrencyExchangeDto>>(repo, map)
{
  public override IEnumerable<CurrencyExchangeDto> Get(BudgetRequest<CurrencyExchangeDto> request, Budget budget)
  => budget.Operations.Where(x => x is CurrencyExchange).Select(x => _mapper.Map<CurrencyExchangeDto>(x as CurrencyExchange));
}
