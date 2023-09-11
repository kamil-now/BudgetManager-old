public record CurrencyExchangeDto(
  string Id,
  string CreatedDate,
  string Title,
  Money Value,
  string Date,
  string Description,
  string AccountId,
  string TargetCurrency,
  decimal ExchangeRate
);