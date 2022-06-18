namespace BudgetManager.Domain.Models;

public class FundTransfer : MoneyOperation
{
  public string? SourceFundId { get; }
  public string? TargetFundId { get; }
  public FundTransfer(
    string title,
     Money value,
     string sourceFundId,
     string targetFundId,
     DateTime date
     ) : base(title, value, date)
  {
    SourceFundId = sourceFundId;
    TargetFundId = targetFundId;
  }
}
