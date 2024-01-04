namespace BudgetManager.Application.Features.BudgetManagement;

using BudgetManager.Infrastructure;

public abstract class BudgetCommandValidator<T> : AbstractValidator<T> where T : IBudgetCommand
{
  protected IUserBudgetRepository repository;
  protected BudgetCommandValidator(IUserBudgetRepository repository)
  {
    this.repository = repository;
    RuleFor(x => x.UserId)
      .MustAsync(async (id, cancellation) => await repository.Exists(id))
      .WithMessage("Budget does not exist.")
      .DependentRules(() => RulesWhenBudgetExists());
  }

  protected virtual void RulesWhenBudgetExists() { }
}
