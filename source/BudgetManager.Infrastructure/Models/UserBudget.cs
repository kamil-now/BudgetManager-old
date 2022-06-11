namespace BudgetManager.Infrastructure.Models;

using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;


public class UserBudget
{
  [BsonId]
  [BsonRepresentation(BsonType.ObjectId)]
  public string? Id { get; set; }
  public string? UserId { get; set; }
  public IEnumerable<Account>? Accounts { get; set; }
  public IEnumerable<Fund>? Funds { get; set; }
  public SpendingFund? SpendingFund { get; set; }
  private IEnumerable<Income>? Incomes { get; set; }
  private IEnumerable<Expense>? Expenses { get; set; }
  private IEnumerable<Allocation>? Allocations { get; set; }
  private IEnumerable<FundTransfer>? FundTransfers { get; set; }

}