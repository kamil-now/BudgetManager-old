namespace BudgetManager.Infrastructure.Models;

public class FundTransferEntity : MoneyOperationEntity
{
  public string? SourceFundId { get; set; }
  public string? TargetFundId { get; set; }
}