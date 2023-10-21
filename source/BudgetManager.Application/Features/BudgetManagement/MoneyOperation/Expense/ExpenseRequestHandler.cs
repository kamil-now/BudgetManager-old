namespace BudgetManager.Application.Features.BudgetManagement;

using AutoMapper;

public class ExpenseRequestHandler : BudgetRequestHandler<ExpenseRequest, ExpenseDto>
{
  public ExpenseRequestHandler(IUserBudgetRepository repo, IMapper map)
   : base(repo, map)
  {
  }

  public override ExpenseDto Get(ExpenseRequest request, Budget budget)
  {
    var expense = budget.Operations.First(x => x.Id == request.ExpenseId) as Expense;
    if (expense is null)
    {
      throw new Exception();
    }
    return _mapper.Map<ExpenseDto>(expense)
     with
    {
      Type = MoneyOperationType.Expense,
      AccountName = budget.Accounts.First(x => x.Id == expense.AccountId).Name,
      FundName = budget.Funds.First(x => x.Id == expense.FundId).Name,
    };
  }
}
