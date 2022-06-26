namespace BudgetManager.Domain.Models;

public class Allocation : MoneyOperation
{
  public string? FundId { get; private set; }
  public string? Category { get; private set; }
  public Allocation(
    string id,
    string title,
    Money value,
    DateOnly date,
    string description,
    DateTime createdDate,
    string? fundId = null,
    string? category = null
    ) : base(id, title, value, date, description, createdDate)
  {
    FundId = fundId;
    Category = category;
  }
}
