namespace BudgetManager.Application.Commands;

using System.Text.Json.Serialization;
using AutoMapper;
using BudgetManager.Domain.Models;
using BudgetManager.Infrastructure;

public record CreateCurrencyExchangeCommand(
  [property: JsonIgnore()] string UserId,
  string Title,
  Money Value,
  string? Date,
  string AccountId,
  string TargetCurrency,
  decimal ExchangeRate,
  string? Description
  ) : IRequest<string>, IBudgetCommand;

public class CreateCurrencyExchangeCommandHandler
  : BudgetCommandHandler<CreateCurrencyExchangeCommand, string>
{
  public CreateCurrencyExchangeCommandHandler(IUserBudgetRepository repo, IMapper map)
  : base(repo, map)
  {
  }

  public override string ModifyBudget(CreateCurrencyExchangeCommand command, Budget budget)
  {
    var id = Guid.NewGuid().ToString();
    var date = DateTime.Now;
    if (command.Date is not null)
    {
      if (DateTime.TryParse(command.Date, out var commandDate))
      {
        date = commandDate;
      }
      else
      {
        throw new Exception("Invalid date.");
      }
    }

    budget.AddOperation(
      new CurrencyExchange(
        id,
        command.Title,
        command.Value,
        command.AccountId,
        command.TargetCurrency,
        command.ExchangeRate,
        date,
        command.Description ?? string.Empty,
        DateTime.Now
        )
      );

    return id;
  }
}

public class CreateCurrencyExchangeCommandValidator
  : BudgetCommandValidator<CreateCurrencyExchangeCommand>
{
  public CreateCurrencyExchangeCommandValidator(IUserBudgetRepository repository, AppConfig appConfig) : base(repository)
  {
    RuleFor(x => x.Title)
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
      return budget!.Accounts?.Any(x => x.Id == command.AccountId && !x.IsDeleted) ?? false;
    }).WithMessage("Account is deleted or does not exist.");
  }
}