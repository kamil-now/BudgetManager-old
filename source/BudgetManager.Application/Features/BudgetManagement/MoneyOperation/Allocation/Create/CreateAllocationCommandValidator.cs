namespace BudgetManager.Application.Features.BudgetManagement;

using BudgetManager.Infrastructure;

public class CreateAllocationCommandValidator
  : BudgetCommandValidator<CreateAllocationCommand>
{
  public CreateAllocationCommandValidator(IUserBudgetRepository repository, AppConfig config) : base(repository)
  {
    RuleFor(x => x.Title)
      .MaximumLength(config.MaxTitleLength);

    RuleFor(x => x.Description)
      .MaximumLength(config.MaxContentLength);

    RuleFor(x => x.Value.Amount)
      .NotEqual(0);
  }

  protected override void RulesWhenBudgetExists()
  {
    RuleFor(x => x)
      .MustAsync(async (command, cancellation)
        => (await repository.Get(command.UserId)).Funds?.Any(x => x.Id == command.TargetFundId && !x.IsDeleted) ?? false)
      .WithMessage("Target fund is deleted or does not exist.");
  }
}
