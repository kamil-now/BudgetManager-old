namespace BudgetManager.Application.Features.BudgetManagement;

using BudgetManager.Infrastructure;

public class RecalculateBudgetCommandValidator(IUserBudgetRepository repository) 
  : BudgetCommandValidator<RecalculateBudgetCommand>(repository)
{
}
