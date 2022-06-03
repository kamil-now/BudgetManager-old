namespace BudgetManager.Domain.Models;

public class Allocation : MoneyOperation
{
  public int FundId { get; }
  public Allocation(string title, Money value, int fundId) : base(title, value)
  {
    FundId = fundId;
  }
}
