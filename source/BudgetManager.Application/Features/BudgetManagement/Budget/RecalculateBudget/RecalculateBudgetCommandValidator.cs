namespace BudgetManager.Application.Features.BudgetManagement;

using BudgetManager.Infrastructure;

public class RecalculateBudgetCommandValidator
  : BudgetCommandValidator<RecalculateBudgetCommand>
{
  public RecalculateBudgetCommandValidator(IUserBudgetRepository repository, AppConfig config) : base(repository)
  {
  }
}
