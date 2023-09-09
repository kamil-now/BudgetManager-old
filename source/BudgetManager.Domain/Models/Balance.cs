namespace BudgetManager.Domain.Models;

public class Balance : Dictionary<string, decimal>
{
  public Balance()
  {

  }
  public Balance(IDictionary<string, decimal> value) : base(value)
  {
    foreach (var key in Keys)
    {
      if (this[key] == 0)
      {
        Remove(key);
      }
    }
  }
  public void Add(Balance balance)
  {
    foreach (var (currency, value) in balance)
    {
      Add(new Money(value, currency));
    }
  }
  public void Deduct(Balance balance)
  {
    foreach (var (currency, value) in balance)
    {
      Deduct(new Money(value, currency));
    }
  }
  public void Add(Money money)
  {
    if (ContainsKey(money.Currency))
    {
      this[money.Currency] += money.Amount;
    }
    else
    {
      Add(money.Currency, money.Amount);
    }
  }
  public void Deduct(Money money)
  {
    if (ContainsKey(money.Currency))
    {
      this[money.Currency] -= money.Amount;
    }
    else
    {
      Add(money.Currency, -money.Amount);
    }
    if (this[money.Currency] == 0)
    {
      Remove(money.Currency);
    }
  }
}
