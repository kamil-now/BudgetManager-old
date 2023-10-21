namespace BudgetManager.Application.Features.BudgetManagement;

using System.Text.Json.Serialization;
using BudgetManager.Domain.Models;

public record CreateCurrencyExchangeCommand(
  [property: JsonIgnore()] string UserId,
  string Title,
  Money Value,
  string Date,
  string AccountId,
  string TargetCurrency,
  decimal ExchangeRate,
  string? Description
  ) : IRequest<string>, IBudgetCommand;
