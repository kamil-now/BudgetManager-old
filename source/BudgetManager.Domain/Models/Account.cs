namespace BudgetManager.Domain.Models;

public class Account
{
  public string? Id { get; }
  public string Name { get; internal set; }
  public Money Balance
  {
    get => _balance;
    private set
    {
      _balance = value;
    }
  }
  public Money InitialBalance { get; private set; }
  private Money _balance;
  public Account(string id, string name, Money initialBalance)
  {
    Id = id;
    Name = name;
    _balance = initialBalance with { };
    InitialBalance = initialBalance with { };
  }

  public void Add(Money money) => Balance = Balance with { Amount = Balance.Amount + money.Amount };
  public void Deduct(Money money) => Balance = Balance with { Amount = Balance.Amount - money.Amount };
}
