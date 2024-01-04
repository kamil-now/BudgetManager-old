namespace BudgetManager.Domain.Models;

public class Expense(
  string id,
  string title,
  Money value,
  string date,
  string accountId,
  string fundId,
  string description,
  DateTime createdDate
  ) : MoneyOperation(id, title, value, date, description, createdDate)
{
  public string AccountId { get; private set; } = accountId;
  public string FundId { get; private set; } = fundId;

  public void Update(string? fundId, string? accountId, string? title, Money? value, string? date, string? description)
  {
    Update(title, value, date, description);
    if (accountId is not null)
    {
      AccountId = accountId;
    }
    if (fundId is not null)
    {
      FundId = fundId;
    }
  }
}
