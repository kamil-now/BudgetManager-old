namespace BudgetManager.Application.Commands;

using System.Text.Json.Serialization;
using AutoMapper;
using BudgetManager.Domain.Models;
using BudgetManager.Infrastructure;

public record CreateIncomeCommand(
  [property: JsonIgnore()] string UserId,
  string Title,
  Money Value,
  string? Date,
  string AccountId,
  string? Description
  ) : IRequest<string>, IBudgetCommand;

public class CreateIncomeCommandHandler
  : BudgetCommandHandler<CreateIncomeCommand, string>
{
  public CreateIncomeCommandHandler(IUserBudgetRepository repo, IMapper map)
  : base(repo, map)
  {
  }

  public override string ModifyBudget(CreateIncomeCommand command, Budget budget)
  {
    var id = Guid.NewGuid().ToString();
    var now = DateOnly.FromDateTime(DateTime.Now);
    var date = command.Date is null ? now : DateOnly.Parse(command.Date);

    budget.AddOperation(
      new Income(
        id,
        command.AccountId,
        command.Title,
        command.Value,
        date,
        command.Description ?? string.Empty,
        DateTime.Now
        )
      );

    return id;
  }
}

public class CreateIncomeCommandValidator
  : BudgetCommandValidator<CreateIncomeCommand>
{
  public CreateIncomeCommandValidator(IUserBudgetRepository repository, AppConfig config) : base(repository)
  {
    RuleFor(x => x.Title)
      .NotEmpty()
      .MaximumLength(config.MaxTitleLength);

    RuleFor(x => x.Description)
      .MaximumLength(config.MaxContentLength);

    RuleFor(x => x.Value.Amount)
      .GreaterThan(0);
  }

  protected override void RulesWhenBudgetExists()
  {
    RuleFor(x => x)
      .MustAsync(async (command, cancellation)
        => (await repository.Get(command.UserId)).Accounts?.Any(x => x.Id == command.AccountId) ?? false)
      .WithMessage("Account does not exist.")
      .DependentRules(() =>
        RuleFor(x => x)
        .MustAsync(async (command, cancellation)
          => (await repository.Get(command.UserId)).Accounts!
              .First(x => x.Id == command.AccountId).Currency == command.Value.Currency)
        .WithMessage("Account currency does not match income currency.")
      );
  }
}