namespace BudgetManager.Application.Features.BudgetManagement;

using AutoMapper;

public class UserSettingsRequestHandler : BudgetRequestHandler<UserSettingsRequest, UserSettingsDto>
{
  public UserSettingsRequestHandler(IUserBudgetRepository repo, IMapper map)
   : base(repo, map)
  {
  }

  public override UserSettingsDto Get(UserSettingsRequest request, Budget budget)
    => _mapper.Map<UserSettingsDto>(budget.UserSettings);
}
