namespace BudgetManager.Domain.Models;

public class FundTransfer(
  string id,
  string title,
  Money value,
  string sourceFundId,
  string targetFundId,
  string date,
  string description,
  DateTime createdDate
  ) : MoneyOperation(id, title, value, date, description, createdDate)
{
  public string SourceFundId { get; private set; } = sourceFundId;
  public string TargetFundId { get; private set; } = targetFundId;

  public void Update(
    string? sourceFundId,
    string? targetFundId,
    string? title,
    Money? value,
    string? date,
    string? description)
  {
    base.Update(title, value, date, description);

    if (sourceFundId is not null)
    {
      SourceFundId = sourceFundId;
    }
    if (targetFundId is not null)
    {
      TargetFundId = targetFundId;
    }
  }
}
