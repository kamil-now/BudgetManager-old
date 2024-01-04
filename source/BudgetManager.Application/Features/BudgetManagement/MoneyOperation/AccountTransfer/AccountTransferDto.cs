namespace BudgetManager.Application.Features.BudgetManagement;

public record AccountTransferDto(
  string? Id = null,
  string? CreatedDate = null,
  string? Title = null,
  Money? Value = null,
  string? Date = null,
  string? Description = null,
  string? AccountId = null,
  string? AccountName = null,
  string? TargetAccountId = null,
  string? TargetAccountName = null,
  MoneyOperationType? Type = null
  );
