namespace BudgetManager.Domain.Models;

public class Allocation(
  string id,
  string title,
  Money value,
  string targetFundId,
  string date,
  string description,
  DateTime createdDate
  ) : MoneyOperation(id, title, value, date, description, createdDate)
{
  public string TargetFundId { get; private set; } = targetFundId;

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
