namespace BudgetManager.Application.Features.BudgetManagement;

public class UserSettingsRequestValidator(IUserBudgetRepository repository) 
  : BudgetRequestValidator<UserSettingsRequest>(repository)
{
}
