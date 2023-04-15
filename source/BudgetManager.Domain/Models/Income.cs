namespace BudgetManager.Domain.Models;

public class Income : MoneyOperation
{
  public string AccountId { get; private set; }
  public string FundId { get; private set; }
  public Income(
    string id,
    string accountId,
    string fundId,
    string title,
    Money value,
    DateOnly date,
    string description,
    DateTime createdDate
    ) : base(id, title, value, date, description, createdDate)
  {
    AccountId = accountId;
    FundId = fundId;
  }

  public void Update(string? accountId, string? fundId, string? title, Money? value, string? date, string? description)
  {
    base.Update(title, value, date, description);
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
