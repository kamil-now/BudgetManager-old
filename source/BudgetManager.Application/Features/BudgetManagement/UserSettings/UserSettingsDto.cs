namespace BudgetManager.Application;

public record UserSettingsDto(IEnumerable<string> AccountsOrder, IEnumerable<string> FundsOrder);