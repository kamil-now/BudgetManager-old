namespace BudgetManager.Infrastructure.Models;

public class Fund
{
  public int Id { get; }
  public string? Name { get; }
  public Dictionary<string, decimal>? Balance { get; set; }
}
