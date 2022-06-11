namespace BudgetManager.Infrastructure.Models;

public class FundTransfer : MoneyOperation
{
  public string? SourceFundId { get; set; }
  public string? TargetFundId { get; set; }
}