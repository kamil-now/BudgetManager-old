namespace BudgetManager.Domain.Models;

public abstract class MoneyOperation
{
  public string Title { get; }
  public Money Value { get; }
  public DateTime Date { get; }
  public string? Description { get; set; }

  public MoneyOperation(string title, Money value)
  {
    Title = title;
    Value = value;
    Date = DateTime.Now;
  }
}
