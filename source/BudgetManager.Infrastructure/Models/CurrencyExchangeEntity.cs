namespace BudgetManager.Infrastructure.Models;

public class CurrencyExchangeEntity : MoneyOperationEntity
{
  public string? AccountId { get; set; }
  public string? TargetCurrency { get; set; }
  public decimal ExchangeRate { get; set; }
}
