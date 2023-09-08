namespace BudgetManager.Domain.Models;

public class Expense : MoneyOperation
{
  public string AccountId { get; private set; }
  public string FundId { get; private set; }

  public Expense(
    string id,
    string title,
    Money value,
    DateOnly date,
    string accountId,
    string fundId,
    string description,
    DateTime createdDate
    ) : base(id, title, value, date, description, createdDate)
  {
    AccountId = accountId;
    FundId = fundId;
  }

  public void Update(string? accountId, string? title, Money? value, string? date, string? description)
  {
    Update(title, value, date, description);
    if (accountId is not null)
    {
      AccountId = accountId;
    }
  }
}
