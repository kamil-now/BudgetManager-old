namespace BudgetManager.Application.Features.BudgetManagement;

using AutoMapper;

public class IncomeRequestHandler(IUserBudgetRepository repo, IMapper map)
  : BudgetRequestHandler<IncomeRequest, IncomeDto>(repo, map)
{
  public override IncomeDto Get(IncomeRequest request, Budget budget)
  {
    if (budget.Operations.First(x => x.Id == request.IncomeId) is not Income income)
    {
      throw new Exception();
    }
    return _mapper.Map<IncomeDto>(income) with
    {
      Type = MoneyOperationType.Income,
      AccountName = budget.Accounts.First(x => x.Id == income.AccountId).Name,
    };
  }
}
