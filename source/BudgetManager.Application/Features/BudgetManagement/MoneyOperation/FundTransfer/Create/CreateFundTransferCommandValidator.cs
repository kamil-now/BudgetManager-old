namespace BudgetManager.Application.Features.BudgetManagement;

using BudgetManager.Application.Extensions;
using BudgetManager.Infrastructure;

public class CreateFundTransferCommandValidator
  : BudgetCommandValidator<CreateFundTransferCommand>
{
  public CreateFundTransferCommandValidator(IUserBudgetRepository repository, AppConfig appConfig) : base(repository)
  {
    RuleFor(x => x.Title)
      .MaximumLength(appConfig.MaxTitleLength);

    RuleFor(x => x.Description)
      .MaximumLength(appConfig.MaxContentLength);

    RuleFor(x => x.Value.Amount)
      .GreaterThan(0);

    RuleFor(x => x.Value.Currency)
      .ISO_4217_Currency();
  }

  protected override void RulesWhenBudgetExists()
  {
    RuleFor(x => x)
      .Must((command, cancellation) => !string.IsNullOrEmpty(command.FundId))
        .WithMessage("Source id must be defined.")
      .Must((command, cancellation) => !string.IsNullOrEmpty(command.TargetFundId))
        .WithMessage("Target id must be defined.")
      .DependentRules(() =>
      {
        RuleFor(x => x)
          .MustAsync(async (command, cancellation) =>
          {
            var budget = await repository.Get(command.UserId);
            return budget!.Funds?.Any(x => x.Id == command.FundId) ?? false;
          })
          .WithMessage(command => $"Source Fund with id '{command.FundId}' does not exist in the budget.")
          .DependentRules(() =>
          {
            RuleFor(x => x)
              .MustAsync(async (command, cancellation) =>
              {
                var budget = await repository.Get(command.UserId);
                var fund = budget!.Funds!.First(x => x.Id == command.FundId);
                return fund!.Balance!.Keys.Contains(command.Value.Currency) && fund!.Balance![command.Value.Currency] >= command.Value.Amount;
              })
              .WithMessage("Insufficient funds.");
          });

        RuleFor(x => x)
          .MustAsync(async (command, cancellation) =>
          {
            var budget = await repository.Get(command.UserId);
            return budget!.Funds?.Any(x => x.Id == command.TargetFundId) ?? false;
          }).WithMessage(command => $"Target fund with id '{command.TargetFundId}' does not exist in the budget.");
      });
  }
}
