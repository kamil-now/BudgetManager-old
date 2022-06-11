namespace BudgetManager.Domain.Models;

public class FundTransfer : MoneyOperation
{
  public int SourceFundId { get; }
  public int TargetFundId { get; }
  public FundTransfer(
    string title,
     Money value,
     int sourceFundId,
     int targetFundId,
     DateTime date
     ) : base(title, value, date)
  {
    SourceFundId = sourceFundId;
    TargetFundId = targetFundId;
  }
}
