namespace BudgetManager.Domain.Models;

public class SpendingFund : Fund
{
  public IEnumerable<SpendingCategory> Categories { get; }
  public SpendingFund(
    IEnumerable<SpendingCategory> categories,
     string id,
     string name
     ) : base(id, name)
  {
    Categories = categories;
  }
}
