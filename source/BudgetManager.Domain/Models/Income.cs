namespace BudgetManager.Domain.Models;

public class Income : MoneyOperation
{
  public int AccountId { get; }
  public Income(
    int accountId,
    string title,
    Money value,
    DateTime date
    ) : base(title, value, date)
  {
    AccountId = accountId;
  }
}
