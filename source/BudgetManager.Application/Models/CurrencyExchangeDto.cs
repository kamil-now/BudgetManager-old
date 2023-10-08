public record CurrencyExchangeDto(
  string? Id = null,
  string? CreatedDate = null,
  string? Title = null,
  Money? Value = null,
  string? Date = null,
  string? Description = null,
  string? AccountId = null,
  string? AccountName = null,
  string? TargetCurrency = null,
  decimal? ExchangeRate = null,
  MoneyOperationType? Type = null
);