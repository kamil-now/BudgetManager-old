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
  string? Date,
  string? AccountId,
  string? FundId,
  string? Description
  ) : IRequest<IncomeDto>, IOperationCommand;

public class UpdateIncomeCommandHandler
  : BudgetCommandHandler<UpdateIncomeCommand, IncomeDto>
{
  public UpdateIncomeCommandHandler(IUserBudgetRepository repo, IMapper map)
  : base(repo, map)
  {
  }

  public override IncomeDto ModifyBudget(UpdateIncomeCommand command, Budget budget)
  {
    var income = budget.Operations.First(x => x.Id == command.OperationId) as Income;

    income!.Update(command.AccountId, command.FundId, command.Title, command.Value, command.Date, command.Description);

    return _mapper.Map<IncomeDto>(income);
  }
}

public class IncomeCommandValidator<T>
  : BudgetCommandValidator<T> where T : IOperationCommand
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

public class UpdateIncomeCommandValidator
  : IncomeCommandValidator<UpdateIncomeCommand>
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