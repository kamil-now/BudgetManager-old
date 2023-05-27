namespace BudgetManager.Domain.Models;

public class Account
{
  public string? Id { get; }
  public string Name { get; internal set; }
  public Money Balance { get; private set; }
  public Account(string id, string name, string currency)
  {
    Id = id;
    Name = name;
    Balance = new Money(0, currency);
  }

  public void Add(Money money) => Balance = Balance with { Amount = Balance.Amount + money.Amount };
  public void Deduct(Money money) => Balance = Balance with { Amount = Balance.Amount - money.Amount };
}
