namespace BudgetManager.Infrastructure.Models;

public class AccountEntity
{
  public string? Id { get; set; }
  public string? Name { get; set; }
  public string? Currency { get; set; }
  public decimal Balance { get; set; }
  public bool IsDeleted { get; set; }
}
