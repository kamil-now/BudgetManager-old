namespace BudgetManager.Infrastructure.Models;

using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Extensions.Migration;

public class BudgetEntity : IVersioned
{
  [BsonId]
  public string? UserId { get; set; }
  public int Version { get; set; }
  public UserSettingsEntity? UserSettings { get; set; }
  public IEnumerable<AccountEntity>? Accounts { get; set; }
  public IEnumerable<FundEntity>? Funds { get; set; }
  public IEnumerable<IncomeEntity>? Incomes { get; set; }
  public IEnumerable<ExpenseEntity>? Expenses { get; set; }
  public IEnumerable<FundTransferEntity>? FundTransfers { get; set; }
  public IEnumerable<AccountTransferEntity>? AccountTransfers { get; set; }
  public IEnumerable<AllocationEntity>? Allocations { get; set; }
  public IEnumerable<CurrencyExchangeEntity>? CurrencyExchanges { get; set; }
}