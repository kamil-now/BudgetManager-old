namespace BudgetManager.Application.Features.BudgetManagement;

using AutoMapper;

public class UserSettingsRequestHandler(IUserBudgetRepository repo, IMapper map) 
  : BudgetRequestHandler<UserSettingsRequest, UserSettingsDto>(repo, map)
{
  public override UserSettingsDto Get(UserSettingsRequest request, Budget budget)
  => _mapper.Map<UserSettingsDto>(budget.UserSettings);
}
