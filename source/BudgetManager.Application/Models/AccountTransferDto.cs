public record AccountTransferDto(
  string Id,
  string CreatedDate,
  string Title,
  Money Value,
  string Date,
  string Description,
  string SourceAccountId,
  string TargetAccountId
);