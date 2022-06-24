namespace BudgetManager.Infrastructure.Models;

public class ExpenseEntity : MoneyOperationEntity
{
  public string? AccountId { get; set; }
  public string? FundId { get; set; }
}
