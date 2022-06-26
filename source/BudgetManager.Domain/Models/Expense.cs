namespace BudgetManager.Domain.Models;

public class Expense : MoneyOperation
{
  public string AccountId { get; private set; }
  public string? FundId { get; private set; }
  public string? Category { get; private set; }
  public bool IsConfirmed { get; internal set; }

  public Expense(
    string id,
    string title,
    Money value,
    DateOnly date,
    string accountId,
    string description,
    bool isConfirmed = true,
    string? fundId = null,
    string? category = null
    ) : base(id, title, value, date, description)
  {
    AccountId = accountId;
    FundId = fundId;
    Category = category;
    IsConfirmed = isConfirmed;
  }

  public void Update(string? accountId, string? title, Money? value, DateOnly? date, string? description)
  {
    base.Update(title, value, date, description);
    if (accountId is not null)
    {
      AccountId = accountId;
    }
  }
}
