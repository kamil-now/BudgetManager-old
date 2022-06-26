namespace BudgetManager.Domain.Models;

public class Income : MoneyOperation
{
  public string? AccountId { get; private set; }
  public Income(
    string id,
    string accountId,
    string title,
    Money value,
    DateOnly date,
    string description
    ) : base(id, title, value, date, description)
  {
    AccountId = accountId;
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
