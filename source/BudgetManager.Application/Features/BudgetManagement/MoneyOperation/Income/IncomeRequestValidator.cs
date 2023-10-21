namespace BudgetManager.Application.Features.BudgetManagement;

public class IncomeRequestValidator : BudgetRequestValidator<IncomeRequest>
{
  public IncomeRequestValidator(IUserBudgetRepository repository) : base(repository)
  {
    RuleFor(x => x)
      .MustAsync(async (request, cancellation) =>
      {
        var budget = await repository.Get(request.UserId);
        return budget!.Incomes?.Any(x => x.Id == request.IncomeId) ?? false;
      }).WithMessage("Income with a given id does not exist in the budget.");
  }
}
