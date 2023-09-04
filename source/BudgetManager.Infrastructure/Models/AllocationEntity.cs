namespace BudgetManager.Infrastructure.Models;

public class AllocationEntity : MoneyOperationEntity
{
  public string? TargetFundId { get; set; }
}
