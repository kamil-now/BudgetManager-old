namespace BudgetManager.Domain.Models;

public class UserSettings
{
  public IEnumerable<string> AccountsOrder { get; }
  public IEnumerable<string> FundsOrder { get; }

  public UserSettings(IEnumerable<string> accountsOrder, IEnumerable<string> fundsOrder)
  {
    AccountsOrder = accountsOrder;
    FundsOrder = fundsOrder;
  }
}
