namespace BudgetManager.Application.Requests;

using AutoMapper;

public record UserSettingsDto(IEnumerable<string> AccountsOrder, IEnumerable<string> FundsOrder);
public record UserSettingsRequest(string UserId) : IBudgetRequest, IRequest<UserSettingsDto>;

public class UserSettingsRequestHandler : BudgetRequestHandler<UserSettingsRequest, UserSettingsDto>
{
  public UserSettingsRequestHandler(IUserBudgetRepository repo, IMapper map)
   : base(repo, map)
  {
  }

  public override UserSettingsDto Get(UserSettingsRequest request, Budget budget)
    => _mapper.Map<UserSettingsDto>(budget.UserSettings);
}


public class UserSettingsRequestValidator : BudgetRequestValidator<UserSettingsRequest>
{
  public UserSettingsRequestValidator(IUserBudgetRepository repository) : base(repository)
  {
  }
}
