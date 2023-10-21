namespace BudgetManager.Application.Features.BudgetManagement;

using BudgetManager.Infrastructure;

public class UpdateAccountCommandValidator : BudgetCommandValidator<UpdateAccountCommand>
{
  public UpdateAccountCommandValidator(IUserBudgetRepository repository) : base(repository)
  {
    RuleFor(x => x)
      .MustAsync(async (command, cancellation) =>
      {
        var budget = await repository.Get(command.UserId);
        return budget!.Accounts?.Any(x => x.Id == command.AccountId) ?? false;
      }).WithMessage("Account with a given id does not exist in the budget");

    RuleFor(x => x.Name)
      .NotEmpty()
      .MaximumLength(50);
  }
}
