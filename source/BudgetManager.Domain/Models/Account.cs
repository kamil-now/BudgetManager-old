namespace BudgetManager.Domain.Models;

public class Account
{
  public string Id { get; }
  public string Name { get; internal set; }
  public Balance Balance { get; private set; }
  public Balance InitialBalance { get; internal set; }
  public bool IsDeleted { get; internal set; }

  public Account(string id, string name, Balance initialBalance, bool isDeleted = false)
  {
    Id = id;
    Name = name;
    Balance = new Balance(initialBalance);
    InitialBalance = new Balance(initialBalance);
    IsDeleted = isDeleted;
  }

  public void Add(Money money) => Balance.Add(money);
  public void Deduct(Money money) => Balance.Deduct(money);
}
