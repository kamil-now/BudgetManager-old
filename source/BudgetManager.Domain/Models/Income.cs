namespace BudgetManager.Domain.Models;

public class Income : MoneyOperation
{
  public string? AccountId { get; }
  public Income(
    string accountId,
    string title,
    Money value,
    DateTime date
    ) : base(title, value, date)
  {
    AccountId = accountId;
  }
}
