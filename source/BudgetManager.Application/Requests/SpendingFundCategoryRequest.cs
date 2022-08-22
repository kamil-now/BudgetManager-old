namespace BudgetManager.Application.Requests;

using AutoMapper;

public record SpendingFundCategoryRequest(string UserId, string CategoryName) : IBudgetRequest, IRequest<Dictionary<string, decimal>>;
public class SpendingFundCategoryRequestHandler : BudgetRequestHandler<SpendingFundCategoryRequest, Dictionary<string, decimal>>
{
  public SpendingFundCategoryRequestHandler(IUserBudgetRepository repo, IMapper map)
   : base(repo, map)
  {
  }

  public override Dictionary<string, decimal> Get(SpendingFundCategoryRequest request, Budget budget)
    => budget.SpendingFund.Categories[request.CategoryName];
}

public class SpendingFundCategoryRequestValidator : BudgetRequestValidator<SpendingFundCategoryRequest>
{
  public SpendingFundCategoryRequestValidator(IUserBudgetRepository repository) : base(repository)
  {
  }
}
