namespace BudgetManager.Application.Features.BudgetManagement;

using BudgetManager.Infrastructure;

public class CreateIncomeCommandValidator
  : BudgetCommandValidator<CreateIncomeCommand>
{
  public CreateIncomeCommandValidator(IUserBudgetRepository repository, AppConfig config) : base(repository)
  {
    RuleFor(x => x.Title)
      .NotEmpty()
      .MaximumLength(config.MaxTitleLength);

    RuleFor(x => x.Description)
      .MaximumLength(config.MaxContentLength);

    RuleFor(x => x.Value.Amount)
      .GreaterThan(0);
  }

  protected override void RulesWhenBudgetExists()
  {
    RuleFor(x => x)
      .MustAsync(async (command, cancellation)
        => (await repository.Get(command.UserId)).Accounts?.Any(x => x.Id == command.AccountId && !x.IsDeleted) ?? false)
      .WithMessage("Account is deleted or does not exist.");
  }
}
