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
    var balance = new Dictionary<string, decimal>();

    foreach (var accountBalance in budget.Accounts.Select(x => x.Balance))
    {
      if (balance.ContainsKey(accountBalance.Currency))
      {
        balance[accountBalance.Currency] += accountBalance.Amount;
      }
      else
      {
        balance.Add(accountBalance.Currency, accountBalance.Amount);
      }
    }

    return new BudgetBalanceDto(new Balance(balance), budget.Unallocated);
  }
}

public class BalanceRequestValidator : BudgetRequestValidator<BalanceRequest>
{
  public BalanceRequestValidator(IUserBudgetRepository repo) : base(repo)
  {
  }
}