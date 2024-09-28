namespace BudgetManager.Application.Features.BudgetManagement;

using BudgetManager.Infrastructure;

public class CreateIncomeDistributionTemplateCommandValidator
  : BudgetCommandValidator<CreateIncomeDistributionTemplateCommand>
{
  public CreateIncomeDistributionTemplateCommandValidator(IUserBudgetRepository repository, AppConfig appConfig) : base(repository)
  {
    RuleFor(x => x.Name)
      .NotEmpty()
      .MaximumLength(appConfig.MaxTitleLength);

    RuleFor(x => x.Rules)
      .NotEmpty()
      .WithMessage("Income distribution template must contain at least 1 rule.");
  }

  protected override void RulesWhenBudgetExists()
  {

    RuleFor(x => x)
    .MustAsync(async (command, cancellation) =>
    {
      var funds = (await repository.Get(command.UserId)).Funds;

      return funds?.Any(x => x.Id == command.DefaultFundId && !x.IsDeleted) ?? false;
    }
    ).WithMessage("Default fund is deleted or does not exist.");

    RuleFor(x => x)
    .MustAsync(async (command, cancellation) =>
    {
      var funds = (await repository.Get(command.UserId)).Funds;

      return command.Rules.All(x => funds?.Any(fund => fund.Id == x.FundId && !fund.IsDeleted) ?? false);
    }
    ).WithMessage("Fund of one of the rules is deleted or does not exist.");
  }
}
