namespace BudgetManager.Domain.Models;

public class Allocation : MoneyOperation
{
  public int FundId { get; }
  public Allocation(
    int fundId,
    string title,
    Money value,
    DateTime date
    ) : base(title, value, date)
  {
    FundId = fundId;
  }
}
