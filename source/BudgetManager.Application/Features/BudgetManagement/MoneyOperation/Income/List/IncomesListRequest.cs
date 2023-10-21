namespace BudgetManager.Application.Features.BudgetManagement;

using AutoMapper;

public class IncomesListRequestHandler : BudgetRequestHandler<BudgetRequest<IncomeDto>, IEnumerable<IncomeDto>>
{
  public IncomesListRequestHandler(IUserBudgetRepository repo, IMapper map)
   : base(repo, map)
  {
  }

  public override IEnumerable<IncomeDto> Get(BudgetRequest<IncomeDto> request, Budget budget)
   => budget.Operations.Where(x => x is Income).Select(x => _mapper.Map<IncomeDto>(x as Income));
}
