namespace BudgetManager.Infrastructure;

using BudgetManager.Infrastructure.Models;

public interface IUserBudgetRepository
{
  Task<string> Create(string userId);
  Task<UserBudget> Get(string userId);
  Task<bool> Exists(string userId);
  Task Update(UserBudget budget);
}