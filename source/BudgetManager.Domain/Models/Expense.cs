namespace BudgetManager.Domain.Models;

public class Expense : MoneyOperation
{
  public string? AccountId { get; }
  public string? FundId { get; }
  public Expense(
    string fundId,
    string accountId,
    string title,
    Money value,
    DateTime date
    ) : base(title, value, date)
  {
    AccountId = accountId;
    FundId = fundId;
  }
}
