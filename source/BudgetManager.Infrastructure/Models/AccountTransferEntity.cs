namespace BudgetManager.Infrastructure.Models;

public class AccountTransferEntity : MoneyOperationEntity
{
  public string? SourceAccountId { get; set; }
  public string? TargetAccountId { get; set; }
}