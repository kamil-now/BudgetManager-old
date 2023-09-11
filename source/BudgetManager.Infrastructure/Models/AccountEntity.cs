namespace BudgetManager.Infrastructure.Models;

public class AccountEntity
{
  public string? Id { get; set; }
  public string? Name { get; set; }
  public Dictionary<string, decimal>? InitialBalance { get; set; }
  public Dictionary<string, decimal>? Balance { get; set; }
  public bool IsDeleted { get; set; }
}
