namespace BudgetManager.Application.Features.BudgetManagement;

using BudgetManager.Infrastructure;

public class UpdateMoneyOperationCommandValidator<T>
  : BudgetCommandValidator<T> where T : IMoneyOperationCommand
{
  public UpdateMoneyOperationCommandValidator(IUserBudgetRepository repository) : base(repository)
  {
    RuleFor(x => x)
      .MustAsync(async (command, cancellation) =>
      {
        var budget = await repository.Get(command.UserId) ?? throw new InvalidOperationException();
        bool existsIn(IEnumerable<MoneyOperationEntity>? operations) => operations?.Any(x => x.Id == command.Id) ?? false;
        return existsIn(budget.Incomes)
        || existsIn(budget.Expenses)
        || existsIn(budget.FundTransfers)
        || existsIn(budget.Allocations)
        || existsIn(budget.AccountTransfers)
        || existsIn(budget.CurrencyExchanges);
      }).WithMessage(command => $"Operation with a id {command.Id} does not exist in the budget");
  }
}
