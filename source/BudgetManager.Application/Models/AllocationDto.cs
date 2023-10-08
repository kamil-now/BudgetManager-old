public record AllocationDto(
  string? Id = null,
  DateTime? CreatedDate = null,
  string? Title = null,
  Money? Value = null,
  DateTime? Date = null,
  string? Description = null,
  string? TargetFundId = null,
  string? TargetFundName = null,
  MoneyOperationType? Type = null
);