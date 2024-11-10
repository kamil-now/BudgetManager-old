namespace BudgetManager.Application.Features.BudgetManagement;

public record BudgetSummaryDto(
  UserSettingsDto UserSettings,
  Balance Balance,
  Balance Unallocated,
  IEnumerable<FundDto> Funds,
  IEnumerable<AccountDto> Accounts,
  IEnumerable<IncomeAllocationTemplateDto> IncomeAllocationTemplates,
  IEnumerable<MoneyOperationDto> Operations
  );
