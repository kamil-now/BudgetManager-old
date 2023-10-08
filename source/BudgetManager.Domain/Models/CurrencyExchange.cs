namespace BudgetManager.Domain.Models;

public class CurrencyExchange : MoneyOperation
{
  public string AccountId { get; private set; }
  public string TargetCurrency { get; private set; }
  public decimal ExchangeRate { get; private set; }

  public Money Result => new(Value.Amount / ExchangeRate, TargetCurrency);

  public CurrencyExchange(
    string id,
    string title,
    Money value,
    string accountId,
    string targetCurrency,
    decimal exchangeRate,
    DateTime date,
    string description,
    DateTime createdDate
    ) : base(id, title, value, date, description, createdDate)
  {
    AccountId = accountId;
    TargetCurrency = targetCurrency;
    ExchangeRate = exchangeRate;
  }

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
