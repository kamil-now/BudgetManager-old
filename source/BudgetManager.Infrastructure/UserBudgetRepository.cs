namespace BudgetManager.Infrastructure;

using BudgetManager.Infrastructure.Models;

internal class UserBudgetRepository : IUserBudgetRepository
{
  private IMongoCollection<BudgetEntity> _collection;
  private IBudgetFactory _budgetFactory;

  public UserBudgetRepository(IMongoCollection<BudgetEntity> collection, IBudgetFactory budgetFactory)
  {
    _collection = collection;
    _budgetFactory = budgetFactory;
  }

  public async Task Create(string userId)
  {
    var doc = _budgetFactory.Create(userId);

    await _collection
      .InsertOneAsync(doc)
      .ContinueWith(t => t.IsCompletedSuccessfully ? t : throw t.Exception ?? new Exception("Failed to persist UserBudget in the database"));
  }

  public async Task CreateWithSampleData(string userId){
    
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