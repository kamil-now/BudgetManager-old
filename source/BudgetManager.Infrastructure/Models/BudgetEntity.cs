namespace BudgetManager.Infrastructure.Models;

using MongoDB.Bson.Serialization.Attributes;


public class BudgetEntity
{
  [BsonId]
  public string? UserId { get; set; }
  public UserSettingsEntity? UserSettings { get; set; }
  public IEnumerable<AccountEntity>? Accounts { get; set; }
  public IEnumerable<FundEntity>? Funds { get; set; }
  public IEnumerable<IncomeEntity>? Incomes { get; set; }
  public IEnumerable<ExpenseEntity>? Expenses { get; set; }
  public IEnumerable<FundTransferEntity>? FundTransfers { get; set; }
  public IEnumerable<AccountTransferEntity>? AccountTransfers { get; set; }
}