namespace BudgetManager.Application.Features.BudgetManagement;

using BudgetManager.Application.Extensions;
using BudgetManager.Infrastructure;

public class UpdateIncomeCommandValidator : UpdateMoneyOperationCommandValidator<UpdateIncomeCommand>
{
  public UpdateIncomeCommandValidator(IUserBudgetRepository repository, AppConfig config) : base(repository)
  {
     RuleFor(x => x.Title)
      .Must(title => title is null || title.Length <= config.MaxTitleLength);

    RuleFor(x => x.Value).ISO_4217_Currency(allowNull: true);

    RuleFor(x => x)
      .MustAsync(async (command, cancellation) =>
      {
        if (command.AccountId is null)
          return true;
        var budget = await repository.Get(command.UserId);
        return budget.Accounts?.Any(x => x.Id == command.AccountId) ?? false;
      }).WithMessage(command => $"Account with id {command.AccountId} does not exist in the budget");
  }
}
