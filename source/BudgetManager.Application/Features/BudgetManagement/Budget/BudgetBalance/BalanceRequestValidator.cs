namespace BudgetManager.Application.Features.BudgetManagement;

public class BalanceRequestValidator(IUserBudgetRepository repo) : BudgetRequestValidator<BalanceRequest>(repo)
{
}
