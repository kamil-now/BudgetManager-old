namespace BudgetManager.Application.Commands;

using AutoMapper;
using BudgetManager.Domain.Models;
using BudgetManager.Infrastructure;

public record DeleteAccountCommand(string UserId, string AccountId) : IRequest<Unit>, IBudgetCommand;

public class DeleteAccountCommandHandler : BudgetCommandHandler<DeleteAccountCommand, Unit>
{
  public DeleteAccountCommandHandler(IUserBudgetRepository repo, IMapper map)
  : base(repo, map)
  {
  }

  public override Unit ModifyBudget(DeleteAccountCommand command, Budget budget)
  {
    budget.RemoveAccount(command.AccountId);
    return Unit.Value;
  }
}

public class DeleteAccountCommandValidator : BudgetCommandValidator<DeleteAccountCommand>
{
  public DeleteAccountCommandValidator(IUserBudgetRepository repository) : base(repository)
  {
    RuleFor(x => x)
      .MustAsync(async (command, cancellation) =>
      {
        var budget = await repository.Get(command.UserId);
        return budget!.Accounts?.Any(x => x.Id == command.AccountId) ?? false;
      }).WithMessage("Account with a given id does not exist in the budget");

    RuleFor(x => x)
      .MustAsync(async (command, cancellation) =>
      {
        var budget = await repository.Get(command.UserId);
        return (budget!.Expenses is null || !budget!.Expenses.Any(x => x.AccountId == command.AccountId))
        && (budget!.Incomes is null || !budget!.Incomes.Any(x => x.AccountId == command.AccountId));
      }).WithMessage("Account cannot be removed because there are budget operations referencing this account");
  }
}