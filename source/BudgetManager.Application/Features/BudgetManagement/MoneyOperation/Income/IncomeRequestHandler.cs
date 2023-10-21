namespace BudgetManager.Application.Features.BudgetManagement;

using AutoMapper;

public class IncomeRequestHandler : BudgetRequestHandler<IncomeRequest, IncomeDto>
{
  public IncomeRequestHandler(IUserBudgetRepository repo, IMapper map)
   : base(repo, map)
  {
  }

  public override IncomeDto Get(IncomeRequest request, Budget budget)
  {
    var income = budget.Operations.First(x => x.Id == request.IncomeId) as Income;
    if (income is null)
    {
      throw new Exception();
    }
    return _mapper.Map<IncomeDto>(income)
     with
    {
      Type = MoneyOperationType.Income,
      AccountName = budget.Accounts.First(x => x.Id == income.AccountId).Name,
    };
  }
}
