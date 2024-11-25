namespace BudgetManager.Infrastructure;

using BudgetManager.Infrastructure.Models;

public interface IUserBudgetRepository
{  
  Task CreateWithSampleData(string userId);
  Task Create(string userId);
  Task<BudgetEntity> Get(string userId);
  Task<bool> Exists(string userId);
  Task Update(BudgetEntity budget);
  Task Delete(string userId);
  Task<bool> TryAcquireLock(string userId);
  Task ReleaseLock(string userId);
}