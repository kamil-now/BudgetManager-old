namespace BudgetManager.Application.Features.BudgetManagement;

using BudgetManager.Domain.Models;

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
  ) : UpdateMoneyOperationCommand<CurrencyExchange, CurrencyExchangeDto>(UserId, OperationId);
