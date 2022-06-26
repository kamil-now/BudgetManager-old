namespace BudgetManager.Application.Commands;

using System.Text.Json.Serialization;
using AutoMapper;
using BudgetManager.Domain.Models;
using BudgetManager.Infrastructure;

public record UpdateIncomeCommand(
  [property: JsonIgnore()]
  string UserId,
  string OperationId,
  string? Title,
  Money? Value,
  DateOnly? Date,
  string? AccountId,
  string? Description
  ) : IRequest<Unit>, IOperationCommand;

public class UpdateIncomeCommandHandler : BudgetCommandHandler<UpdateIncomeCommand, Unit>
{
  public UpdateIncomeCommandHandler(IUserBudgetRepository repo, IMapper map)
  : base(repo, map)
  {
  }

  public override Unit ModifyBudget(UpdateIncomeCommand command, Budget budget)
  {
    var Income = budget.Operations.First(x => x.Id == command.OperationId) as Income;

    Income!.Update(command.AccountId, command.Title, command.Value, command.Date, command.Description);

    return Unit.Value;
  }
}


public class IncomeCommandValidator<T> : BudgetCommandValidator<T> where T : IRequest<Unit>, IOperationCommand
{
  public IncomeCommandValidator(IUserBudgetRepository repository) : base(repository)
  {
    RuleFor(x => x)
      .MustAsync(async (command, cancellation) =>
      {
        var budget = await repository.Get(command.UserId);
        return budget!.Incomes?.Any(x => x.Id == command.OperationId) ?? false;
      }).WithMessage("Income with a given id does not exist in the budget");
  }
}

public class UpdateIncomeCommandValidator : IncomeCommandValidator<UpdateIncomeCommand>
{
  public UpdateIncomeCommandValidator(IUserBudgetRepository repository) : base(repository)
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