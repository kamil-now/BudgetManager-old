namespace BudgetManager.Infrastructure;

using BudgetManager.Infrastructure.Models;
using DnsClient.Protocol;

internal class UserBudgetRepository : IUserBudgetRepository
{
  private IMongoCollection<BudgetEntity> _collection;

  public UserBudgetRepository(IMongoCollection<BudgetEntity> collection) => _collection = collection;

  public async Task<string> Create(string userId)
  {
    var doc = new BudgetEntity()
    {
      UserId = userId,
      SpendingFund = new SpendingFundEntity()
      {
        Id = Guid.NewGuid().ToString(),
        Name = "Spending Fund",
        InitialBalance = new Dictionary<string, decimal>()
      }
    };
    return await _collection
      .InsertOneAsync(doc)
      .ContinueWith(t => t.IsCompletedSuccessfully ? doc.UserId : throw t.Exception ?? new Exception("Failed to create UserBudget"));
  }

  public async Task<BudgetEntity> Get(string userId)
    => await _collection
    .FindAsync(b => b.UserId == userId)
    .ContinueWith(t => t.IsCompletedSuccessfully ? t.Result.First() : throw t.Exception ?? new Exception("Failed to retrieve UserBudget"));

  public async Task<ReplaceOneResult> Update(BudgetEntity budget)
    => await _collection.ReplaceOneAsync(x => x.UserId == budget.UserId, budget);

  public async Task<bool> Exists(string userId)
    => await _collection
    .FindAsync(b => b.UserId == userId)
    .ContinueWith(t => t.IsCompletedSuccessfully ? t.Result.Any() : throw t.Exception ?? new Exception("Failed to retrieve UserBudget"));

}