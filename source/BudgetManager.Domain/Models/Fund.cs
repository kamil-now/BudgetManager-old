namespace BudgetManager.Domain.Models;

public class Fund
{
  public string? Id { get; }
  public string Name { get; internal set; }
  public Balance Balance { get; }

  public Fund(string id, string name)
  {
    Id = id;
    Name = name;
    Balance = new Balance();
  }
  public void Add(Money money) => Balance.Add(money);
  public void Deduct(Money money) => Balance.Deduct(money);
}
