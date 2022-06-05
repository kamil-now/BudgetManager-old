namespace BudgetManager.Domain.Models;

public class Expense : MoneyOperation
{
  public int AccountId { get; }
  public int FundId { get; }
  public Expense(
    int fundId,
    int accountId,
    string title,
    Money value,
    DateTime date
    ) : base(title, value, date)
  {
    AccountId = accountId;
    FundId = fundId;
  }
}
