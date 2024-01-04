namespace BudgetManager.Domain.Models;

public class Fund(string id, string name, bool isDeleted = false)
{
  public string? Id { get; } = id;
  public Balance Balance { get; } = [];
  public string Name { get; internal set; } = name;
  public bool IsDeleted { get; internal set; } = isDeleted;

  public void Add(Money money) => Balance.Add(money);
  public void Deduct(Money money) => Balance.Deduct(money);
}
