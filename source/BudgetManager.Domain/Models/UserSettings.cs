namespace BudgetManager.Domain.Models;

public class UserSettings(IEnumerable<string> accountsOrder, IEnumerable<string> fundsOrder)
{
  public IEnumerable<string> AccountsOrder { get; } = accountsOrder;
  public IEnumerable<string> FundsOrder { get; } = fundsOrder;
}
