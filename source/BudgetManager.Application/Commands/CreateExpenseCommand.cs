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
  string? Description,
  string? FundId,
  string? Category
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
    var date = command.Date is null ? now : DateOnly.Parse(command.Date);

    budget.AddOperation(
      new Expense(
        id,
        command.Title,
        command.Value,
        date,
        command.AccountId,
        command.Description ?? string.Empty,
        DateTime.Now,
        date <= now,
        command.FundId,
        command.Category
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
          .Must((command, cancellation)
          => !string.IsNullOrEmpty(command.Category) ^ !string.IsNullOrEmpty(command.FundId)
          ).WithMessage("Either Fund id or Spending Fund category must be defined.");

        RuleFor(x => x)
        .MustAsync(async (command, cancellation)
          => (await repository.Get(command.UserId)).Accounts!
              .First(x => x.Id == command.AccountId).Currency == command.Value.Currency)
        .WithMessage("Account currency does not match expense currency.");

        RuleFor(x => x)
          .MustAsync(async (command, cancellation) =>
          {
            if (command.FundId is null)
              return true;
            var budget = await repository.Get(command.UserId);
            return budget!.Funds?.Any(x => x.Id == command.FundId) ?? false;
          }).WithMessage("Fund does not exist.");
      });

  }
}