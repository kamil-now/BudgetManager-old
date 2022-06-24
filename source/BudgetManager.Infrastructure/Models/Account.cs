namespace BudgetManager.Infrastructure.Models;

public class AccountEntity
{
  public string? Id { get; set; }
  public string? Name { get; set; }
  public decimal InitialAmount { get; set; }
  public string? Currency { get; set; }
}
