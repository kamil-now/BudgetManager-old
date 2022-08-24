using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BudgetManager.Infrastructure;
using BudgetManager.Infrastructure.Models;

public class MockUserBudgetRepository : IUserBudgetRepository
{
  private Dictionary<string, BudgetEntity> _db = new Dictionary<string, BudgetEntity>();
  private BudgetFactory _budgetFactory = new BudgetFactory();
  public Task Create(string userId)
  {
    _db[userId] = _budgetFactory.Create(userId);
    return Task.CompletedTask;
  }

  public Task<bool> Exists(string userId) => Task.FromResult(_db.Keys.Any(x => x == userId));

  public Task<BudgetEntity> Get(string userId) => Task.FromResult(_db[userId]);

  public Task Update(BudgetEntity budget)
  {
    _db[budget.UserId!] = budget;
    return Task.CompletedTask;
  }

  public async Task Delete(string userId)
  {
    if (await Exists(userId))
    {
      _db.Remove(userId);
    }
  }
}
