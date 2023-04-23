using BudgetManager.Infrastructure.Models;

public interface IBudgetFactory
{
  BudgetEntity Create(string userId, string defaultFundName);
}