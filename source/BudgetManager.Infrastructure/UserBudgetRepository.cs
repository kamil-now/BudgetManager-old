namespace BudgetManager.Infrastructure;

using BudgetManager.Infrastructure.Models;

internal class UserBudgetRepository(IMongoCollection<BudgetEntity> _collection, IBudgetFactory _budgetFactory) : IUserBudgetRepository
{
  public async Task<bool> TryAcquireLock(string userId)
  {
    var filter = Builders<BudgetEntity>.Filter.And(
        Builders<BudgetEntity>.Filter.Eq("_id", userId),
        Builders<BudgetEntity>.Filter.Or(
          Builders<BudgetEntity>.Filter.Eq("IsLocked", false),
          Builders<BudgetEntity>.Filter.Exists("IsLocked", false)
        )
    );

    var update = Builders<BudgetEntity>.Update
        .Set("IsLocked", true)
        .Set("LockTimestamp", DateTime.UtcNow);

    var options = new FindOneAndUpdateOptions<BudgetEntity>
    {
      ReturnDocument = ReturnDocument.After
    };

    var result = await _collection.FindOneAndUpdateAsync(filter, update, options);

    return result != null;
  }
  public async Task ReleaseLock(string userId)
  {
    var filter = Builders<BudgetEntity>.Filter.Eq("_id", userId);

    var update = Builders<BudgetEntity>.Update
        .Set("IsLocked", false)
        .Unset("LockTimestamp");

    await _collection.UpdateOneAsync(filter, update);
  }
  public async Task Create(string userId)
  {
    var doc = _budgetFactory.Create(userId);

    await _collection
      .InsertOneAsync(doc)
      .ContinueWith(t => t.IsCompletedSuccessfully ? t : throw t.Exception ?? new Exception("Failed to persist UserBudget in the database"));
  }

  public async Task CreateWithSampleData(string userId)
  {

    var doc = _budgetFactory.CreateWithSampleData(userId);

    await _collection
      .InsertOneAsync(doc)
      .ContinueWith(t => t.IsCompletedSuccessfully ? t : throw t.Exception ?? new Exception("Failed to persist UserBudget in the database"));
  }

  public async Task Update(BudgetEntity budget)
    => await _collection.ReplaceOneAsync(x => x.UserId == budget.UserId, budget);

  public async Task Delete(string userId)
    => await _collection.DeleteOneAsync(x => x.UserId == userId);

  public async Task<BudgetEntity> Get(string userId) => await Get(userId, b => b.First());

  public async Task<bool> Exists(string userId) => await Get(userId, b => b.Any());

  private async Task<T> Get<T>(string userId, Func<IAsyncCursor<BudgetEntity>, T> func) => await _collection
    .FindAsync(b => b.UserId == userId)
    .ContinueWith(t => t.IsCompletedSuccessfully ? func(t.Result) : throw t.Exception ?? new Exception("Failed to retrieve UserBudget from the database"));
}