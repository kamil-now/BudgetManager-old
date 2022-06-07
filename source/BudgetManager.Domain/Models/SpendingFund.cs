namespace BudgetManager.Domain.Models;

public class SpendingFund : Fund
{
  public IEnumerable<SpendingCategory> Categories { get; }
  public SpendingFund(
    IEnumerable<SpendingCategory> categories,
     int id,
     string name
     ) : base(id, name)
  {
    Categories = categories;
  }
}
