namespace BudgetManager.Application.Features.BudgetManagement;

using AutoMapper;

public class ExpensesListRequestHandler : BudgetRequestHandler<BudgetRequest<ExpenseDto>, IEnumerable<ExpenseDto>>
{
  public ExpensesListRequestHandler(IUserBudgetRepository repo, IMapper map)
   : base(repo, map)
  {
  }

  public override IEnumerable<ExpenseDto> Get(BudgetRequest<ExpenseDto> request, Budget budget)
   => budget.Operations.Where(x => x is Expense).Select(x => _mapper.Map<ExpenseDto>(x as Expense));
}
