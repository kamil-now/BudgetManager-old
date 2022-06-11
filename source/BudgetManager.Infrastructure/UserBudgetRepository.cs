using BudgetManager.Infrastructure.Models;
using MongoDB.Driver;

namespace BudgetManager.Infrastructure;

internal class UserBudgetRepository : IUserBudgetRepository
{
  private IMongoCollection<UserBudget> _collection;

  public UserBudgetRepository(IMongoCollection<UserBudget> collection) => _collection = collection;

  public void Create(string userId)
     => _collection.InsertOne(new UserBudget() { UserId = userId });

  public void Get(string userId)
    => _collection.Find(b => b.UserId == userId);

  public void Update(UserBudget budget)
    => _collection.ReplaceOne(b => b.UserId == b.UserId, budget);
}