namespace BudgetManager.Infrastructure.Models;

public class Expense : MoneyOperation
{
  public string? AccountId { get; set; }
  public string? FundId { get; set; }
}
