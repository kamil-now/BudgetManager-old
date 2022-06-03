namespace BudgetManager.Domain.Models;

public class Fund
{
  public int Id { get; }
  public string Name { get; }
  public Balance Balance { get; } = new Balance();

  public Fund(int id, string name)
  {
    Id = id;
    Name = name;
  }
  public void Add(Money money) => Balance.Add(money);
  public void Deduct(Money money) => Balance.Deduct(money);
}
