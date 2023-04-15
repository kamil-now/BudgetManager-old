namespace BudgetManager.Infrastructure.Models;

public class ExpenseEntity : MoneyOperationEntity
{
  public string AccountId { get; set; } = null!;
  public string FundId { get; set; } = null!;
  public bool IsConfirmed { get; set; }
}
