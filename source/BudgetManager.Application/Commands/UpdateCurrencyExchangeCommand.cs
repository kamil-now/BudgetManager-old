namespace BudgetManager.Application.Commands;

using AutoMapper;
using BudgetManager.Domain.Models;
using BudgetManager.Infrastructure;

public record UpdateCurrencyExchangeCommand(
  string UserId,
  string OperationId,
  string? Title,
  Money? Value,
  string? Date,
  string? AccountId,
  string? TargetCurrency,
  decimal? ExchangeRate,
  string? Description
  ) : UpdateOperationCommand<CurrencyExchange, CurrencyExchangeDto>(UserId, OperationId);

public class UpdateCurrencyExchangeCommandHandler : UpdateOperationCommandHandler<UpdateCurrencyExchangeCommand, CurrencyExchange, CurrencyExchangeDto>
{
  public UpdateCurrencyExchangeCommandHandler(IUserBudgetRepository repo, IMapper map)
  : base(repo, map)
  {
  }

  protected override void Update(CurrencyExchange operation, UpdateCurrencyExchangeCommand command)
    => operation.Update(
        command.AccountId,
        command.TargetCurrency,
        command.ExchangeRate,
        command.Title,
        command.Value,
        command.Date,
        command.Description
      );
}

public class UpdateCurrencyExchangeCommandValidator : UpdateOperationCommandValidator<UpdateCurrencyExchangeCommand>
{
  public UpdateCurrencyExchangeCommandValidator(IUserBudgetRepository repository, AppConfig config) : base(repository)
  {
    RuleFor(x => x.Title)
      .Must(title => title is null || title.Length <= config.MaxTitleLength);

    RuleFor(x => x.Value).ISO_4217_Currency(allowNull: true);

    RuleFor(x => x)
      .MustAsync(async (command, cancellation) =>
      {
        if (command.AccountId is null)
          return true;
        var budget = await repository.Get(command.UserId);
        return budget.Accounts?.Any(x => x.Id == command.AccountId) ?? false;
      }).WithMessage(command => $"Account with id {command.AccountId} does not exist in the budget");
  }
}