public record FundTransferDto(
  string? Id = null,
  DateTime? CreatedDate = null,
  string? Title = null,
  Money? Value = null,
  DateTime? Date = null,
  string? Description = null,
  string? FundId = null,
  string? FundName = null,
  string? TargetFundId = null,
  string? TargetFundName = null,
  MoneyOperationType? Type = null
);