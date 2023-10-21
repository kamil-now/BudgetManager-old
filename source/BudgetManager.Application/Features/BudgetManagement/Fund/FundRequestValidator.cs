namespace BudgetManager.Application.Features.BudgetManagement;

public class FundRequestValidator : BudgetRequestValidator<FundRequest>
{
  public FundRequestValidator(IUserBudgetRepository repository) : base(repository)
  {
    RuleFor(x => x)
      .MustAsync(async (request, cancellation) =>
      {
        var budget = await repository.Get(request.UserId);
        return budget!.Funds?.Any(x => x.Id == request.FundId) ?? false;
      }).WithMessage("Fund with a given id does not exist in the budget.");
  }
}
