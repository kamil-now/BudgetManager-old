namespace BudgetManager.Application.Features.BudgetManagement;

using AutoMapper;

public class ExpenseRequestHandler(IUserBudgetRepository repo, IMapper map)
  : BudgetRequestHandler<ExpenseRequest, ExpenseDto>(repo, map)
{
  public override ExpenseDto Get(ExpenseRequest request, Budget budget)
  {
    if (budget.Operations.First(x => x.Id == request.ExpenseId) is not Expense expense)
    {
      throw new Exception();
    }
    return _mapper.Map<ExpenseDto>(expense) with
    {
      Type = MoneyOperationType.Expense,
      AccountName = budget.Accounts.First(x => x.Id == expense.AccountId).Name,
      FundName = budget.Funds.First(x => x.Id == expense.FundId).Name,
    };
  }
}
