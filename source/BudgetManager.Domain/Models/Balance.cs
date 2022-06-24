namespace BudgetManager.Domain.Models;

public class Balance : Dictionary<string, decimal>
{
  internal Balance()
  {

  }
  public Balance(IDictionary<string, decimal> value) : base(value)
  {
  }
  public void Add(Money money) => this[money.Currency] += money.Amount;
  public void Deduct(Money money) => this[money.Currency] -= money.Amount;
}
