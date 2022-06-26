namespace BudgetManager.Domain.Models;

public class FundTransfer : MoneyOperation
{
  public string? SourceFundId { get; }
  public string? TargetFundId { get; }
  public FundTransfer(
    string id,
    string title,
    Money value,
    string sourceFundId,
    string targetFundId,
    DateOnly date,
    string description
    ) : base(id, title, value, date, description)
  {
    SourceFundId = sourceFundId;
    TargetFundId = targetFundId;
  }
}
