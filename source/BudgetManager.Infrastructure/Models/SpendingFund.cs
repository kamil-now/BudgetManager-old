namespace BudgetManager.Infrastructure.Models;

public class SpendingFundEntity : FundEntity
{
  public IEnumerable<SpendingCategoryEntity>? Categories { get; set; }
}
