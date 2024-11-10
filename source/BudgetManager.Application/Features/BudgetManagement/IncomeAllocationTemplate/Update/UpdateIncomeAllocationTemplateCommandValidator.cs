namespace BudgetManager.Application.Features.BudgetManagement;

using BudgetManager.Infrastructure;

public class UpdateIncomeAllocationTemplateCommandValidator : BudgetCommandValidator<UpdateIncomeAllocationTemplateCommand>
{
  public UpdateIncomeAllocationTemplateCommandValidator(IUserBudgetRepository repository, AppConfig config) : base(repository)
  {
    RuleFor(x => x.Name)
      .Must(name => name is null || name.Length <= config.MaxTitleLength);

    RuleFor(x => x.Rules)
      .Must(rules => rules is null || rules.Any())
      .WithMessage("Income allocation template must contain at least 1 rule.");
  }

  protected override void RulesWhenBudgetExists()
  {
    RuleFor(x => x)
    .MustAsync(async (command, cancellation) =>
    {
      if (command.DefaultFundId is null)
        return true;

      var funds = (await repository.Get(command.UserId)).Funds;

      return funds?.Any(x => x.Id == command.DefaultFundId && !x.IsDeleted) ?? false;
    }
    ).WithMessage("Default fund is deleted or does not exist.");

    RuleFor(x => x)
    .MustAsync(async (command, cancellation) =>
    {
      if (command.Rules is null)
        return true;

      var funds = (await repository.Get(command.UserId)).Funds;

      return command.Rules.All(x => funds?.Any(fund => fund.Id == x.FundId && !fund.IsDeleted) ?? false);
    }
    ).WithMessage("Fund of one of the rules is deleted or does not exist.");
  }
}
