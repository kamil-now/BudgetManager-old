namespace BudgetManager.Infrastructure.Models;

public class MoneyOperationEntity
{
  public string? Id { get; set; }
  public string? Title { get; set; }
  public decimal Amount { get; set; }
  public string? Currency { get; set; }
  public DateOnly Date { get; set; }
  public DateTime CreatedDate { get; set; }
  public string? Description { get; set; }
}
