namespace BudgetManager.Application.Features.BudgetManagement;

using BudgetManager.Infrastructure;

public class DeleteAccountCommandValidator : BudgetCommandValidator<DeleteAccountCommand>
{
  public DeleteAccountCommandValidator(IUserBudgetRepository repository) : base(repository)
  {
  }
  protected override void RulesWhenBudgetExists()
  {
    RuleFor(x => x)
    .MustAsync(async (command, cancellation) =>
    {
      var budget = await repository.Get(command.UserId);
      return budget!.Accounts?.Any(x => x.Id == command.AccountId) ?? false;
    }).WithMessage("Account with a given id does not exist in the budget")
    .DependentRules(() =>
    {

      RuleFor(x => x)
          .MustAsync(async (command, cancellation) =>
          {
            var budget = await repository.Get(command.UserId);
            return budget!.Accounts!.First(x => x.Id == command.AccountId)!.Balance!.Values.All(x => x == 0);
          }).WithMessage("Account must be empty in order to be deleted.");
    });
  }
}
