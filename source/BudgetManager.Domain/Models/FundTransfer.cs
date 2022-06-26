namespace BudgetManager.Domain.Models;

public class FundTransfer : MoneyOperation
{
  public string SourceFundId { get; private set; }
  public string? TargetFundId { get; private set; }
  public string? Category { get; private set; }

  public FundTransfer(
    string id,
    string title,
    Money value,
    string sourceFundId,
    string? targetFundId,
    DateOnly date,
    string description,
    DateTime createdDate,
    string? category
    ) : base(id, title, value, date, description, createdDate)
  {
    SourceFundId = sourceFundId;
    TargetFundId = targetFundId;
    Category = category;
  }

  public void Update(string? sourceFundId, string? targetFundId, string? category, string? title, Money? value, string? date, string? description)
  {
    base.Update(title, value, date, description);

    if (sourceFundId is not null)
    {
      SourceFundId = sourceFundId;
    }
    if (targetFundId is not null)
    {
      TargetFundId = targetFundId;
      Category = null;
    }
    if (category is not null)
    {
      TargetFundId = null;
      Category = category;
    }
  }
}
