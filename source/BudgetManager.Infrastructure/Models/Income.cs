namespace BudgetManager.Infrastructure.Models;

public class IncomeEntity : MoneyOperationEntity
{
  public string? AccountId { get; set; }
  public string? FundId { get; set; }
}
