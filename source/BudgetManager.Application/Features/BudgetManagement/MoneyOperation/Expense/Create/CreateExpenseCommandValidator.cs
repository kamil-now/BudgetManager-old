namespace BudgetManager.Application.Features.BudgetManagement;

using BudgetManager.Infrastructure;

public class CreateExpenseCommandValidator
  : BudgetCommandValidator<CreateExpenseCommand>
{
  public CreateExpenseCommandValidator(IUserBudgetRepository repository, AppConfig appConfig) : base(repository)
  {
    RuleFor(x => x.Title)
      .NotEmpty()
      .MaximumLength(appConfig.MaxTitleLength);

    RuleFor(x => x.Description)
      .MaximumLength(appConfig.MaxContentLength);

    RuleFor(x => x.Value.Amount)
      .GreaterThan(0);
  }

  protected override void RulesWhenBudgetExists()
  {
    RuleFor(x => x)
    .MustAsync(async (command, cancellation) =>
    {
      var budget = await repository.Get(command.UserId);
      return budget!.Accounts?.Any(x => x.Id == command.AccountId && !x.IsDeleted) ?? false;
    }).WithMessage("Account is deleted or does not exist.");

    RuleFor(x => x)
    .MustAsync(async (command, cancellation)
      => (await repository.Get(command.UserId)).Funds?.Any(x => x.Id == command.FundId && !x.IsDeleted) ?? false)
    .WithMessage("Fund is deleted or does not exist.");
  }
}
