namespace BudgetManager.Infrastructure.Models;

public class SpendingFund : Fund
{
  public IEnumerable<SpendingCategory>? Categories { get; set; }
}
