namespace BudgetManager.Domain.Models;

public abstract class MoneyOperation
{
  public string Title { get; }
  public Money Value { get; }
  public DateTime Date { get; }
  public DateTime CreatedDate { get; }
  public string? Description { get; set; }

  public MoneyOperation(
    string title,
    Money value,
    DateTime date
    )
  {
    Title = title;
    Value = value;
    Date = date;
    CreatedDate = DateTime.Now;
  }
}
