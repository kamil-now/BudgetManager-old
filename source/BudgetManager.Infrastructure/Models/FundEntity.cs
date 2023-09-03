namespace BudgetManager.Infrastructure.Models;

public class FundEntity
{
  public string? Id { get; set; }
  public string? Name { get; set; }
  public Dictionary<string, decimal>? Balance { get; set; }
  public bool IsDefault { get; set; }
  public bool IsDeleted { get; set; }
}
