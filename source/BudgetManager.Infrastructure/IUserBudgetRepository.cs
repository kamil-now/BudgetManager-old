using BudgetManager.Infrastructure.Models;

namespace BudgetManager.Infrastructure;

public interface IUserBudgetRepository
{
  void Create(string userId);
  void Get(string userId);
  void Update(UserBudget budget);
}