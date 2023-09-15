public record BudgetSummaryDto(
  UserSettingsDto UserSettings,
  Balance Balance,
  Balance Unallocated,
  IEnumerable<FundDto> Funds,
  IEnumerable<AccountDto> Accounts,
  IEnumerable<MoneyOperationDto> Operations
  );