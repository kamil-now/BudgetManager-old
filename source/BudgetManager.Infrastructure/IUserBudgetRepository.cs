namespace BudgetManager.Infrastructure;

using BudgetManager.Infrastructure.Models;

public interface IUserBudgetRepository
{
  Task<string> Create(string userId);
  Task<BudgetEntity> Get(string userId);
  Task<bool> Exists(string userId);
  Task<ReplaceOneResult> Update(BudgetEntity budget);
}