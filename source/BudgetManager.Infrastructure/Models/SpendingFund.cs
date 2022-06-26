namespace BudgetManager.Infrastructure.Models;

public class SpendingFundEntity : FundEntity
{
  public Dictionary<string, Dictionary<string, decimal>>? Categories { get; set; }
}
