namespace BudgetManager.Application.Features.BudgetManagement;

using AutoMapper;

public class IncomesListRequestHandler(IUserBudgetRepository repo, IMapper map) 
  : BudgetRequestHandler<BudgetRequest<IncomeDto>, IEnumerable<IncomeDto>>(repo, map)
{
  public override IEnumerable<IncomeDto> Get(BudgetRequest<IncomeDto> request, Budget budget)
  => budget.Operations.Where(x => x is Income).Select(x => _mapper.Map<IncomeDto>(x as Income));
}
