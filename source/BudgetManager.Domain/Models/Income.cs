namespace BudgetManager.Domain.Models;

public class Income : MoneyOperation
{
  public int AccountId { get; }
  public Income(string title, Money value, int accountId) : base(title, value)
  {
    AccountId = accountId;
  }
}
