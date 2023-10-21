namespace BudgetManager.Application.Features.BudgetManagement;

public record BudgetBalanceDto(
  Balance Balance,
  Balance Unallocated
  );
  