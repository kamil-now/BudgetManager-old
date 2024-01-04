namespace BudgetManager.Application.Features.BudgetManagement;

public record IncomeDto(
  string? Id = null,
  string? CreatedDate = null,
  string? Title = null,
  Money? Value = null,
  string? Date = null,
  string? Description = null,
  string? AccountId = null,
  string? AccountName = null,
  MoneyOperationType? Type = null
  );