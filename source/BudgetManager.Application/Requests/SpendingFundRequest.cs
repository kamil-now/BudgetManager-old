namespace BudgetManager.Application.Requests;

using AutoMapper;

public record SpendingFundRequest(string UserId) : IBudgetRequest, IRequest<SpendingFundDto>;
public record SpendingFundDto(string Id, string Name, Balance Balance, Dictionary<string, Balance> Categories);

public class SpendingFundRequestHandler : BudgetRequestHandler<SpendingFundRequest, SpendingFundDto>
{
  public SpendingFundRequestHandler(IUserBudgetRepository repo, IMapper map)
   : base(repo, map)
  {
  }

  public override SpendingFundDto Get(SpendingFundRequest request, Budget budget)
  {
    var unallocatedFunds = budget.GetUnallocatedFunds(false);
    var spendingFund = _mapper.Map<SpendingFundDto>(budget.SpendingFund);
    foreach (var category in spendingFund.Categories)
    {
      foreach (var (currency, amount) in category.Value)
      {
        unallocatedFunds.Deduct(new Money(amount, currency));
      }
    }
    return spendingFund with { Balance = unallocatedFunds };
  }
}

public class SpendingFundRequestValidator : BudgetRequestValidator<SpendingFundRequest>
{
  public SpendingFundRequestValidator(IUserBudgetRepository repository) : base(repository)
  {
  }
}
