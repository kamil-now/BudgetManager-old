namespace BudgetManager.Domain.Models;

public class Fund
{
  public string? Id { get; }
  public string Name { get; }
  public Balance Balance { get; }
  public Balance InitialBalance { get; }

  public Fund(string id, string name, Balance initialBalance)
  {
    Id = id;
    Name = name;
    InitialBalance = initialBalance;
    Balance = new Balance(initialBalance.ToDictionary(x => x.Key, y => y.Value));
  }
  public void Add(Money money) => Balance.Add(money);
  public void Deduct(Money money) => Balance.Deduct(money);
}
