namespace BudgetManager.Domain.Models;

public class AccountTransfer : MoneyOperation
{
  public string SourceAccountId { get; private set; }
  public string TargetAccountId { get; private set; }

  public AccountTransfer(
    string id,
    string title,
    Money value,
    string sourceAccountId,
    string targetAccountId,
    DateOnly date,
    string description,
    DateTime createdDate
    ) : base(id, title, value, date, description, createdDate)
  {
    SourceAccountId = sourceAccountId;
    TargetAccountId = targetAccountId;
  }

  public void Update(
    string? sourceAccountId, 
    string? targetAccountId, 
    string? category, 
    string? title, 
    Money? value, 
    string? date, 
    string? description)
  {
    base.Update(title, value, date, description);

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
