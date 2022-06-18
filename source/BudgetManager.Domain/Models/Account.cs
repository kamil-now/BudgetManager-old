namespace BudgetManager.Domain.Models;

public class Account
{
  public string? Id { get; }
  public string Name { get; }
  public Money Balance { get; private set; }

  public Account(string id, string name, Money balance)
  {
    Id = id;
    Name = name;
    Balance = balance;
  }

  public void Add(Money money) => Balance = Balance with { Amount = Balance.Amount + money.Amount };
  public void Deduct(Money money) => Balance = Balance with { Amount = Balance.Amount - money.Amount };
}
