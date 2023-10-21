namespace BudgetManager.Application.Features.BudgetManagement;

public class BudgetSummaryRequestValidator : AbstractValidator<BudgetSummaryRequest>
{
  public BudgetSummaryRequestValidator(IUserBudgetRepository repository)
  {
    RuleFor(x => x.UserId)
      .MustAsync(async (id, cancellation) => await repository.Exists(id))
        .WithMessage("Budget does not exist.");
  }
}
