namespace BudgetManager.Domain.Models;

public class Income(
  string id,
  string accountId,
  string title,
  Money value,
  string date,
  string description,
  DateTime createdDate
  ) : MoneyOperation(id, title, value, date, description, createdDate)
{
  public string AccountId { get; private set; } = accountId;

  public void Update(string? accountId, string? title, Money? value, string? date, string? description)
  {
    Update(title, value, date, description);
    if (accountId is not null)
    {
      AccountId = accountId;
    }
  }
}
