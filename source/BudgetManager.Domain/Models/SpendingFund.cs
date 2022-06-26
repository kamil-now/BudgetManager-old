namespace BudgetManager.Domain.Models;

public class SpendingFund : Fund
{
  public Dictionary<string, Balance> Categories { get; }
  public SpendingFund(
    Dictionary<string, Balance> categories,
    string id,
    string name,
    Balance initialBalance
  ) : base(id, name, initialBalance)
  {
    Categories = categories ?? new Dictionary<string, Balance>();
  }
}
