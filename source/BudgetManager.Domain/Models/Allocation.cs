namespace BudgetManager.Domain.Models;

public class Allocation : MoneyOperation
{
  public string TargetFundId { get; private set; }

  public Allocation(
    string id,
    string title,
    Money value,
    string targetFundId,
    DateTime date,
    string description,
    DateTime createdDate
    ) : base(id, title, value, date, description, createdDate)
  {
    TargetFundId = targetFundId;
  }

  public void Update(
    string? targetFundId,
    string? title,
    Money? value,
    string? date,
    string? description)
  {
    Update(title, value, date, description);

    if (targetFundId is not null)
    {
      TargetFundId = targetFundId;
    }
  }
}
