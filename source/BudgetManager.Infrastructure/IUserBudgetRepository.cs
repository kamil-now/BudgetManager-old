namespace BudgetManager.Infrastructure;

using BudgetManager.Infrastructure.Models;

public interface IUserBudgetRepository
{
  Task Create(string userId);
  Task<BudgetEntity> Get(string userId);
  Task<bool> Exists(string userId);
  Task Update(BudgetEntity budget);
  Task Delete(string userId);
}