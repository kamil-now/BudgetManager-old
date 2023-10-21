namespace BudgetManager.Application.Features.BudgetManagement;

public class BalanceRequestValidator : BudgetRequestValidator<BalanceRequest>
{
  public BalanceRequestValidator(IUserBudgetRepository repo) : base(repo)
  {
  }
}
