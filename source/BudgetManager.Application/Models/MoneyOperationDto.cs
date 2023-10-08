public record MoneyOperationDto(
  MoneyOperationType Type,
  string Id,
  string CreatedDate,
  string Title,
  Money Value,
  string Date,
  string Description,
  string? AccountId = null,
  string? AccountName = null,
  string? FundId = null,
  string? FundName = null,
  string? TargetFundId = null,
  string? TargetFundName = null,
  string? TargetAccountId = null,
  string? TargetAccountName = null,
  string? TargetCurrency = null,
  decimal? ExchangeRate = null
);