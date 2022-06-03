namespace BudgetManager.Domain.Models;

public class Expense : MoneyOperation
{
  public int AccountId { get; }
  public int FundId { get; }
  public Expense(
    string title,
     Money value,
     int accountId,
     int fundId
     ) : base(title, value)
  {
    AccountId = accountId;
    FundId = fundId;
  }
}
