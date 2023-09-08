namespace BudgetManager.Domain.Models;

public class Account
{
  public string Id { get; }
  public string Name { get; internal set; }
  public Money Balance { get; private set; }
  public Money InitialBalance { get; internal set; }
  public bool IsDeleted { get; internal set; }

  public Account(string id, string name, Money initialBalance)
  {
    Id = id;
    Name = name;
    Balance = new Money(initialBalance.Amount, initialBalance.Currency);
    InitialBalance = new Money(initialBalance.Amount, initialBalance.Currency);
  }

  public void Add(Money money) => Balance = Balance with { Amount = Balance.Amount + money.Amount };
  public void Deduct(Money money) => Balance = Balance with { Amount = Balance.Amount - money.Amount };
}
