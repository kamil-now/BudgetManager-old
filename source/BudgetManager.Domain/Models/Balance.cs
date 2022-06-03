namespace BudgetManager.Domain.Models;

public class Balance : Dictionary<string, decimal>
{
  public void Add(Money money) => this[money.Currency] += money.Amount;
  public void Deduct(Money money) => this[money.Currency] -= money.Amount;
}
