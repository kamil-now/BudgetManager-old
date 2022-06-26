namespace BudgetManager.Application.Commands;

using System.Text.Json.Serialization;
using AutoMapper;
using BudgetManager.Domain.Models;
using BudgetManager.Infrastructure;

public record CreateExpenseCommand(
  [property: JsonIgnore()] string UserId, string Title, Money Value, DateOnly? Date, string AccountId, string? Description, string? FundId, string? Category)
  : IRequest<string>, IBudgetCommand;

public class CreateExpenseCommandHandler : BudgetCommandHandler<CreateExpenseCommand, string>
{
  public CreateExpenseCommandHandler(IUserBudgetRepository repo, IMapper map)
  : base(repo, map)
  {
  }

  public override string ModifyBudget(CreateExpenseCommand command, Budget budget)
  {
    var id = Guid.NewGuid().ToString();
    var now = DateOnly.FromDateTime(DateTime.Now);
    var date = command.Date ?? now;

    budget.AddOperation(
      new Expense(
        id,
        command.Title,
        command.Value,
        date,
        command.AccountId,
        command.Description ?? string.Empty,
        date <= now,
        command.FundId,
        command.Category
        )
      );

    return id;
  }
}

public class CreateExpenseCommandValidator : BudgetCommandValidator<CreateExpenseCommand>
{
  public CreateExpenseCommandValidator(IUserBudgetRepository repository) : base(repository)
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

    RuleFor(x => x)
      .MustAsync(async (command, cancellation) =>
      {
        if (command.FundId is null)
          return true;
        var budget = await repository.Get(command.UserId);
        return budget!.Funds?.Any(x => x.Id == command.FundId) ?? false;
      }).WithMessage("Fund with a given id does not exist in the budget");

    RuleFor(x => x)
      .MustAsync(async (command, cancellation) =>
      {
        if (command.Category is null)
          return true;
        var budget = await repository.Get(command.UserId);
        return budget!.SpendingFund!.Categories?.Any(x => x.Key == command.Category) ?? false;
      }).WithMessage("Category with a given id does not exist in the budget");
  }
}