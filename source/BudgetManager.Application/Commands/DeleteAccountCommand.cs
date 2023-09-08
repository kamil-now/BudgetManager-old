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
        return budget!.Accounts!.First(x => x.Id == command.AccountId)!.Balance == 0;
      }).WithMessage("Account must be empty in order to be deleted.");
    });
  }
}