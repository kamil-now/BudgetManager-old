namespace BudgetManager.Application.Features.BudgetManagement;

using AutoMapper;
using BudgetManager.Domain.Models;
using BudgetManager.Infrastructure;

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

    budget.AddOperation(
      new CurrencyExchange(
        id,
        command.Title,
        command.Value,
        command.AccountId,
        command.TargetCurrency,
        command.ExchangeRate,
        command.Date,
        command.Description ?? string.Empty,
        DateTime.Now
        )
      );

    return id;
  }
}
