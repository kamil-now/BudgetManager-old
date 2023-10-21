namespace BudgetManager.Application.Features.BudgetManagement;

using BudgetManager.Infrastructure;

public class UpdateExpenseCommandValidator : UpdateMoneyOperationCommandValidator<UpdateExpenseCommand>
{
  public UpdateExpenseCommandValidator(IUserBudgetRepository repository, AppConfig config) : base(repository)
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

    RuleFor(x => x)
      .MustAsync(async (command, cancellation) =>
      {
        if (command.FundId is null)
          return true;
        var budget = await repository.Get(command.UserId);
        return budget.Funds?.Any(x => x.Id == command.FundId) ?? false;
      }).WithMessage(command => $"Fund with id {command.FundId} does not exist in the budget");
  }
}
