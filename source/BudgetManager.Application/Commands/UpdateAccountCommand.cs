namespace BudgetManager.Application.Commands;

using AutoMapper;
using BudgetManager.Domain.Models;
using BudgetManager.Infrastructure;

public record UpdateAccountCommand(string UserId, string AccountId, string Name) : IRequest<Unit>, IBudgetCommand;

public class UpdateAccountCommandHandler : BudgetCommandHandler<UpdateAccountCommand, Unit>
{
  public UpdateAccountCommandHandler(IUserBudgetRepository repo, IMapper map)
  : base(repo, map)
  {
  }

  public override Unit ModifyBudget(UpdateAccountCommand command, Budget budget)
  {
    budget.RenameAccount(command.AccountId, command.Name);

    return Unit.Value;
  }
}

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