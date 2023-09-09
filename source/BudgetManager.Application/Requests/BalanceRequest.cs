namespace BudgetManager.Application.Requests;

using AutoMapper;

public record BalanceRequest(string UserId)
  : IBudgetRequest, IRequest<BudgetBalanceDto>;

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

    return new BudgetBalanceDto(balance, budget.Unallocated);
  }
}

public class BalanceRequestValidator : BudgetRequestValidator<BalanceRequest>
{
  public BalanceRequestValidator(IUserBudgetRepository repo) : base(repo)
  {
  }
}