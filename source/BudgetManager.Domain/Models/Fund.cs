namespace BudgetManager.Domain.Models;

public class Fund
{
  public string? Id { get; }
  public Balance Balance { get; }
  public string Name { get; internal set; }
  public bool IsDeleted { get; internal set; }

  public Fund(string id, string name, bool isDeleted = false)
  {
    Id = id;
    Name = name;
    Balance = new Balance();
    IsDeleted = isDeleted;
  }
  public void Add(Money money) => Balance.Add(money);
  public void Deduct(Money money) => Balance.Deduct(money);
}
