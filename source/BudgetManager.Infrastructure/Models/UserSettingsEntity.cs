namespace BudgetManager.Infrastructure.Models;

public class UserSettingsEntity
{
  public IEnumerable<string>? AccountsOrder { get; set;  }
  public IEnumerable<string>? FundsOrder { get; set; }
}
