namespace BudgetManager.Application.Requests;

using AutoMapper;

public record SpendingCategoryRequest(string UserId, string CategoryName) : IBudgetRequest, IRequest<Dictionary<string, decimal>>;
public class SpendingCategoryRequestHandler : BudgetRequestHandler<SpendingCategoryRequest, Dictionary<string, decimal>>
{
  public SpendingCategoryRequestHandler(IUserBudgetRepository repo, IMapper map)
   : base(repo, map)
  {
  }

  public override Dictionary<string, decimal> Get(SpendingCategoryRequest request, Budget budget)
    => budget.SpendingFund.Categories[request.CategoryName];
}

public class SpendingCategoryRequestValidator : BudgetRequestValidator<SpendingCategoryRequest>
{
  public SpendingCategoryRequestValidator(IUserBudgetRepository repository) : base(repository)
  {
  }
}
