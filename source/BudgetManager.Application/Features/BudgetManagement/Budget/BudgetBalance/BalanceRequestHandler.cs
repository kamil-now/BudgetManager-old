namespace BudgetManager.Application.Features.BudgetManagement;

using AutoMapper;

public class BalanceRequestHandler
  : BudgetRequestHandler<BalanceRequest, BudgetBalanceDto>
{
  public BalanceRequestHandler(IUserBudgetRepository repo, IMapper map)
   : base(repo, map)
  {
  }

  public override BudgetBalanceDto Get(BalanceRequest request, Budget budget)
  {
    var balance = new Balance();

    foreach (var accountBalance in budget.Accounts.Select(x => x.Balance))
    {
      balance.Add(accountBalance);
    }

    var unallocated = new Balance(balance);

    foreach (var fundBalance in budget.Funds.Select(x => x.Balance))
    {
      unallocated.Deduct(fundBalance);
    }

    return new BudgetBalanceDto(balance, unallocated);
  }
}
