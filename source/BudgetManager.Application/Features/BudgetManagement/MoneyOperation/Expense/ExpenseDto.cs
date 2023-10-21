public record ExpenseDto(
  string? Id = null,
  string? CreatedDate = null,
  string? Title = null,
  Money? Value = null,
  string? Date = null,
  string? Description = null,
  string? AccountId = null,
  string? AccountName = null,
  string? FundId = null,
  string? FundName = null,
  MoneyOperationType? Type = null
);