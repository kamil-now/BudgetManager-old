namespace BudgetManager.Application.Features.BudgetManagement;

using BudgetManager.Infrastructure;

public class DeleteIncomeDistributionTemplateCommandValidator(IUserBudgetRepository repository) : BudgetCommandValidator<DeleteIncomeDistributionTemplateCommand>(repository)
{
  protected override void RulesWhenBudgetExists()
  {
    RuleFor(x => x)
    .MustAsync(async (command, cancellation) =>
    {
      var budget = await repository.Get(command.UserId);
      return budget!.IncomeDistributionTemplates?.Any(x => x.Id == command.IncomeDistributionTemplateId) ?? false;
    }).WithMessage("Income distribution template with a given id does not exist in the budget.");
  }
}
