namespace BudgetManager.Domain.Models;

public class AccountTransfer(
  string id,
  string title,
  Money value,
  string sourceAccountId,
  string targetAccountId,
  string date,
  string description,
  DateTime createdDate
  ) : MoneyOperation(id, title, value, date, description, createdDate)
{
  public string SourceAccountId { get; private set; } = sourceAccountId;
  public string TargetAccountId { get; private set; } = targetAccountId;

  public void Update(
    string? sourceAccountId,
    string? targetAccountId,
    string? title,
    Money? value,
    string? date,
    string? description)
  {
    Update(title, value, date, description);

    if (sourceAccountId is not null)
    {
      SourceAccountId = sourceAccountId;
    }
    if (targetAccountId is not null)
    {
      TargetAccountId = targetAccountId;
    }
  }
}
