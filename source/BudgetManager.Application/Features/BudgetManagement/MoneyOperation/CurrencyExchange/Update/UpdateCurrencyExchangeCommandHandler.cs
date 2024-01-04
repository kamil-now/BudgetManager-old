namespace BudgetManager.Application.Features.BudgetManagement;

using AutoMapper;
using BudgetManager.Domain.Models;
using BudgetManager.Infrastructure;

public class UpdateCurrencyExchangeCommandHandler(IUserBudgetRepository repo, IMapper map) 
  : UpdateMoneyOperationCommandHandler<UpdateCurrencyExchangeCommand, CurrencyExchange, CurrencyExchangeDto>(repo, map)
{
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
  protected override CurrencyExchangeDto CompleteDto(CurrencyExchangeDto dto, Budget budget)
    => dto with
    {
      Type = MoneyOperationType.CurrencyExchange,
      AccountName = budget.Accounts.First(x => x.Id == dto.AccountId).Name,
    };
}
