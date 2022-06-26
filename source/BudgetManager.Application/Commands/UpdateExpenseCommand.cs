namespace BudgetManager.Application.Commands;

using System.Text.Json.Serialization;
using AutoMapper;
using BudgetManager.Domain.Models;
using BudgetManager.Infrastructure;

public record UpdateExpenseCommand(
  [property: JsonIgnore()]
  string UserId,
  string OperationId,
  string? Title,
  Money? Value,
  string? Date,
  string? AccountId,
  string? Description
  ) : IRequest<Unit>, IOperationCommand;

public class UpdateExpenseCommandHandler
  : BudgetCommandHandler<UpdateExpenseCommand, Unit>
{
  public UpdateExpenseCommandHandler(IUserBudgetRepository repo, IMapper map)
  : base(repo, map)
  {
  }

  public override Unit ModifyBudget(UpdateExpenseCommand command, Budget budget)
  {
    var expense = budget.Operations.First(x => x.Id == command.OperationId) as Expense;

    expense!.Update(command.AccountId, command.Title, command.Value, command.Date, command.Description);

    return Unit.Value;
  }
}


public class ExpenseCommandValidator<T>
  : BudgetCommandValidator<T> where T : IRequest<Unit>, IOperationCommand
{
  public ExpenseCommandValidator(IUserBudgetRepository repository) : base(repository)
  {
    RuleFor(x => x)
      .MustAsync(async (command, cancellation) =>
      {
        var budget = await repository.Get(command.UserId);
        return budget!.Expenses?.Any(x => x.Id == command.OperationId) ?? false;
      }).WithMessage("Expense with a given id does not exist in the budget");
  }
}

public class UpdateExpenseCommandValidator
  : ExpenseCommandValidator<UpdateExpenseCommand>
{
  public UpdateExpenseCommandValidator(IUserBudgetRepository repository) : base(repository)
  {
    RuleFor(x => x.Title)
      .Must(title => title is null || title.Length <= 50);

    RuleFor(x => x.Value)
      .Must(value => value is null || value?.Currency.Length == 3);

    RuleFor(x => x)
      .MustAsync(async (command, cancellation) =>
      {
        if (command.AccountId is null)
          return true;
        var budget = await repository.Get(command.UserId);
        return budget!.Accounts?.Any(x => x.Id == command.AccountId) ?? false;
      }).WithMessage("Account with a given id does not exist in the budget");
  }
}