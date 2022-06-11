namespace BudgetManager.Infrastructure.Models;

public class MoneyOperation
{
  public string? Title { get; set; }
  public decimal Amount { get; set; }
  public string? Currency { get; set; }
  public DateTime Date { get; set; }
  public DateTime CreatedDate { get; set; }
  public string? Description { get; set; }
}
