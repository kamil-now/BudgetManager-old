namespace BudgetManager.Application.Features.BudgetManagement;

using BudgetManager.Infrastructure;

public class DeleteFundCommandValidator(IUserBudgetRepository repository) : BudgetCommandValidator<DeleteFundCommand>(repository)
{
  protected override void RulesWhenBudgetExists()
  {
    RuleFor(x => x)
    .MustAsync(async (command, cancellation) =>
    {
      var budget = await repository.Get(command.UserId);
      return budget!.Funds?.Any(x => x.Id == command.FundId) ?? false;
    }).WithMessage("Fund with a given id does not exist in the budget.")
    .DependentRules(() =>
    {
      RuleFor(x => x)
        .MustAsync(async (command, cancellation) =>
        {
          var budget = await repository.Get(command.UserId);
          return budget!.Funds!.First(x => x.Id == command.FundId)!.Balance!.Values.All(x => x == 0);
        }).WithMessage("Fund must be empty in order to be deleted.");
    });
  }
}
