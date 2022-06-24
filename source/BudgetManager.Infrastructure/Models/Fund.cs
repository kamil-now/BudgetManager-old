namespace BudgetManager.Infrastructure.Models;

public class FundEntity
{
  public string? Id { get; set; }
  public string? Name { get; set; }
  public Dictionary<string, decimal>? InitialBalance { get; set; }
}
