namespace BudgetManager.Domain.Models;

public class Allocation : MoneyOperation
{
  public string? FundId { get; }
  public Allocation(
    string fundId,
    string title,
    Money value,
    DateTime date
    ) : base(title, value, date)
  {
    FundId = fundId;
  }
}
