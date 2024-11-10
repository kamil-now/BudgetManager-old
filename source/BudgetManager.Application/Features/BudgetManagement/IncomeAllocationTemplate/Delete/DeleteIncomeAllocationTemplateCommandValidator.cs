namespace BudgetManager.Application.Features.BudgetManagement;

using BudgetManager.Infrastructure;

public class DeleteIncomeAllocationTemplateCommandValidator(IUserBudgetRepository repository) : BudgetCommandValidator<DeleteIncomeAllocationTemplateCommand>(repository)
{
  protected override void RulesWhenBudgetExists()
  {
    RuleFor(x => x)
    .MustAsync(async (command, cancellation) =>
    {
      var budget = await repository.Get(command.UserId);
      return budget!.IncomeAllocationTemplates?.Any(x => x.Id == command.IncomeAllocationTemplateId) ?? false;
    }).WithMessage("Income allocation template with a given id does not exist in the budget.");
  }
}
