namespace BudgetManager.Domain.Models;

public class CurrencyExchange(
  string id,
  string title,
  Money value,
  string accountId,
  string targetCurrency,
  decimal exchangeRate,
  string date,
  string description,
  DateTime createdDate
  ) : MoneyOperation(id, title, value, date, description, createdDate)
{
  public string AccountId { get; private set; } = accountId;
  public string TargetCurrency { get; private set; } = targetCurrency;
  public decimal ExchangeRate { get; private set; } = exchangeRate;

  public Money Result => new(Math.Round(Value.Amount / ExchangeRate, 2), TargetCurrency);

  public void Update(
    string? accountId,
    string? targetCurrency,
    decimal? exchangeRate,
    string? title,
    Money? value,
    string? date,
    string? description)
  {
    Update(title, value, date, description);

    if (accountId is not null)
    {
      AccountId = accountId;
    }
    if (targetCurrency is not null)
    {
      TargetCurrency = targetCurrency;
    }
    if (exchangeRate is not null)
    {
      ExchangeRate = (decimal)exchangeRate;
    }
  }
}
