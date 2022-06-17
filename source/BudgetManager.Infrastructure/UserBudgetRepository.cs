using BudgetManager.Infrastructure.Models;
using MongoDB.Bson;

namespace BudgetManager.Infrastructure;

internal class UserBudgetRepository : IUserBudgetRepository
{
  private IMongoCollection<UserBudget> _collection;

  public UserBudgetRepository(IMongoCollection<UserBudget> collection) => _collection = collection;

  public async Task<string> Create(string userId)
  {
    var doc = new UserBudget() { UserId = userId, Id = ObjectId.GenerateNewId().ToString() };
    return await _collection
      .InsertOneAsync(doc)
      .ContinueWith(t => t.IsCompletedSuccessfully ? doc.Id : throw t.Exception ?? new Exception("Failed to create UserBudget"));
  }

  public async Task<UserBudget> Get(string userId)
    => await _collection
    .FindAsync(b => b.UserId == userId)
    .ContinueWith(t => t.IsCompletedSuccessfully ? t.Result.First() : throw t.Exception ?? new Exception("Failed to retrieve UserBudget"));

  public async Task Update(UserBudget budget)
    => await _collection.ReplaceOneAsync(b => b.UserId == b.UserId, budget);

  public async Task<bool> Exists(string userId)
    => await _collection
    .FindAsync(b => b.UserId == userId)
    .ContinueWith(t => t.IsCompletedSuccessfully ? t.Result.Any() : throw t.Exception ?? new Exception("Failed to retrieve UserBudget"));

}