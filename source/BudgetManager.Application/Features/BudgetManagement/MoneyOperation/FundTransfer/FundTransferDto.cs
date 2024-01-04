namespace BudgetManager.Application.Features.BudgetManagement;

public record FundTransferDto(
  string? Id = null,
  string? CreatedDate = null,
  string? Title = null,
  Money? Value = null,
  string? Date = null,
  string? Description = null,
  string? FundId = null,
  string? FundName = null,
  string? TargetFundId = null,
  string? TargetFundName = null,
  MoneyOperationType? Type = null
  );