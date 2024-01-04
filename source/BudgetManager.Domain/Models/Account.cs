namespace BudgetManager.Domain.Models;

public class Account(string id, string name, Balance initialBalance, bool isDeleted = false)
{
  public string Id { get; } = id;
  public string Name { get; internal set; } = name;
  public Balance Balance { get; private set; } = new Balance(initialBalance);
  public Balance InitialBalance { get; internal set; } = new Balance(initialBalance);
  public bool IsDeleted { get; internal set; } = isDeleted;

  public void Add(Money money) => Balance.Add(money);
  public void Deduct(Money money) => Balance.Deduct(money);
}
