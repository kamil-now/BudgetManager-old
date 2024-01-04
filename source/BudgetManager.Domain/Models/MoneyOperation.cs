namespace BudgetManager.Domain.Models;

public abstract class MoneyOperation(
  string id,
  string title,
  Money value,
  string date,
  string description,
  DateTime createdDate
  )
{
  public string Id { get; } = id;
  public DateTime CreatedDate { get; } = createdDate;
  public string Title { get; private set; } = title;
  public Money Value { get; private set; } = value;
  public string Date { get; private set; } = date;
  public string Description { get; private set; } = description;

  protected void Update(
    string? title,
    Money? value,
    string? date,
    string? description)
  {
    if (title is not null)
    {
      Title = title;
    }

    if (value is not null)
    {
      Value = (Money)value;
    }

    if (date is not null)
    {
      Date = date;
    }

    if (description is not null)
    {
      Description = description;
    }
  }
}
