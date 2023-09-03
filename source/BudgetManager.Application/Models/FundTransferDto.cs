public record FundTransferDto(
  string Id,
  string CreatedDate,
  string Title,
  Money Value,
  string Date,
  string Description,
  string SourceFundId,
  string TargetFundId
);