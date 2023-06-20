namespace BudgetManager.Application.Commands;

using System.Text.Json.Serialization;
using AutoMapper;
using BudgetManager.Domain.Models;
using BudgetManager.Infrastructure;

public record CreateExpenseCommand(
  [property: JsonIgnore()] string UserId,
  string Title,
  Money Value,
  string? Date,
  string AccountId,
  string FundId,
  string? Description
  ) : IRequest<string>, IBudgetCommand;

public class CreateExpenseCommandHandler
  : BudgetCommandHandler<CreateExpenseCommand, string>
{
  public CreateExpenseCommandHandler(IUserBudgetRepository repo, IMapper map)
  : base(repo, map)
  {
  }

  public override string ModifyBudget(CreateExpenseCommand command, Budget budget)
  {
    var id = Guid.NewGuid().ToString();
    var now = DateOnly.FromDateTime(DateTime.Now);
    var date = command.Date is null ? now : DateOnly.FromDateTime(DateTime.Parse(command.Date));

    budget.AddOperation(
      new Expense(
        id,
        command.Title,
        command.Value,
        date,
        command.AccountId,
        command.FundId,
        command.Description ?? string.Empty,
        DateTime.Now,
        date <= now
        )
      );

    return id;
  }
}

public class CreateExpenseCommandValidator
  : BudgetCommandValidator<CreateExpenseCommand>
{
  public CreateExpenseCommandValidator(IUserBudgetRepository repository, AppConfig appConfig) : base(repository)
  {
    RuleFor(x => x.Title)
      .NotEmpty()
      .MaximumLength(appConfig.MaxTitleLength);

    RuleFor(x => x.Description)
      .MaximumLength(appConfig.MaxContentLength);

    RuleFor(x => x.Value.Amount)
      .GreaterThan(0);
  }

  protected override void RulesWhenBudgetExists()
  {
    RuleFor(x => x)
    .MustAsync(async (command, cancellation) =>
    {
      var budget = await repository.Get(command.UserId);
      return budget!.Accounts?.Any(x => x.Id == command.AccountId) ?? false;
    }).WithMessage("Account does not exist.")
      .DependentRules(() =>
      {
        RuleFor(x => x)
        .MustAsync(async (command, cancellation)
          => (await repository.Get(command.UserId)).Accounts!
              .First(x => x.Id == command.AccountId).Currency == command.Value.Currency)
        .WithMessage("Account currency does not match expense currency.");
      });

    RuleFor(x => x)
    .MustAsync(async (command, cancellation)
      => (await repository.Get(command.UserId)).Funds?.Any(x => x.Id == command.FundId) ?? false)
    .WithMessage("Fund does not exist.");
  }
}