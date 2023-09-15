public record MoneyOperationDto(
  MoneyOperationType Type,
  string Id,
  string CreatedDate,
  string Title,
  Money Value,
  string Date,
  string Description,
  string? AccountId,
  string? AccountName,
  string? FundId,
  string? FundName,
  string? TargetFundId,
  string? TargetFundName,
  string? TargetAccountId,
  string? TargetAccountName,
  string? TargetCurrency,
  decimal? ExchangeRate
);