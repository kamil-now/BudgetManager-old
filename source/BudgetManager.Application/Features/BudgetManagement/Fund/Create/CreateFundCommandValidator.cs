namespace BudgetManager.Application.Features.BudgetManagement;

using BudgetManager.Infrastructure;

public class CreateFundCommandValidator : BudgetCommandValidator<CreateFundCommand>
{
  public CreateFundCommandValidator(IUserBudgetRepository repository, AppConfig appConfig) : base(repository)
  {
    RuleFor(x => x.Name)
      .NotEmpty()
      .MaximumLength(appConfig.MaxTitleLength);
  }
}
