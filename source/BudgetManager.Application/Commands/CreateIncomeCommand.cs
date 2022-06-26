namespace BudgetManager.Application.Commands;

using System.Text.Json.Serialization;
using AutoMapper;
using BudgetManager.Domain.Models;
using BudgetManager.Infrastructure;

public record CreateIncomeCommand(
  [property: JsonIgnore()] string UserId, string Title, Money Value, DateOnly? Date, string AccountId, string? Description)
  : IRequest<string>, IBudgetCommand;

public class CreateIncomeCommandHandler : BudgetCommandHandler<CreateIncomeCommand, string>
{
  public CreateIncomeCommandHandler(IUserBudgetRepository repo, IMapper map)
  : base(repo, map)
  {
  }

  public override string ModifyBudget(CreateIncomeCommand command, Budget budget)
  {
    var id = Guid.NewGuid().ToString();
    var now = DateOnly.FromDateTime(DateTime.Now);
    var date = command.Date ?? now;

    budget.AddOperation(
      new Income(
        id,
        command.AccountId,
        command.Title,
        command.Value,
        date,
        command.Description ?? string.Empty
        )
      );

    return id;
  }
}

public class CreateIncomeCommandValidator : BudgetCommandValidator<CreateIncomeCommand>
{
  public CreateIncomeCommandValidator(IUserBudgetRepository repository) : base(repository)
  {
    RuleFor(x => x.Title)
      .NotEmpty()
      .MaximumLength(50);

    RuleFor(x => x)
      .MustAsync(async (command, cancellation) =>
      {
        var budget = await repository.Get(command.UserId);
        return budget!.Accounts?.Any(x => x.Id == command.AccountId) ?? false;
      }).WithMessage("Account with a given id does not exist in the budget");
  }
}