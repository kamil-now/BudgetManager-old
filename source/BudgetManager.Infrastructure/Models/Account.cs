namespace BudgetManager.Infrastructure.Models;

public class Account
{
  public string? Id { get; set; }
  public string? Name { get; set; }
  public decimal Amount { get; set; }
  public string? Currency { get; set; }
}
