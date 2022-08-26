namespace BudgetManager.Domain.Models;

public class SpendingFund : Fund
{
  public Dictionary<string, Balance> Categories { get; }
  public SpendingFund(
    Dictionary<string, Balance> categories,
    string id,
    string name
  ) : base(id, name)
  {
    Categories = categories ?? new Dictionary<string, Balance>();
  }
}
