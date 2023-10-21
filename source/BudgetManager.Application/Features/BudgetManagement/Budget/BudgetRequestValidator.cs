namespace BudgetManager.Application.Features.BudgetManagement;

public abstract class BudgetRequestValidator<T> : AbstractValidator<T> where T : IBudgetRequest
{
  protected BudgetRequestValidator(IUserBudgetRepository repository)
  {
    RuleFor(x => x.UserId)
      .MustAsync(async (id, cancellation) => await repository.Exists(id))
        .WithMessage("Budget does not exist.");
  }
}
