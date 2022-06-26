namespace BudgetManager.Infrastructure.Models;

public class AllocationEntity : MoneyOperationEntity
{
  public string? FundId { get; set; }

  public string? Category { get; set; }
}
