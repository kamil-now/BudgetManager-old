namespace BudgetManager.Application.Features.BudgetManagement;

using AutoMapper;

public class ExpensesListRequestHandler(IUserBudgetRepository repo, IMapper map) 
  : BudgetRequestHandler<BudgetRequest<ExpenseDto>, IEnumerable<ExpenseDto>>(repo, map)
{
  public override IEnumerable<ExpenseDto> Get(BudgetRequest<ExpenseDto> request, Budget budget)
  => budget.Operations.Where(x => x is Expense).Select(x => _mapper.Map<ExpenseDto>(x as Expense));
}
