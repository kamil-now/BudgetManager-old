namespace BudgetManager.Application.Features.BudgetManagement;

public class UserSettingsRequestValidator : BudgetRequestValidator<UserSettingsRequest>
{
  public UserSettingsRequestValidator(IUserBudgetRepository repository) : base(repository)
  {
  }
}
