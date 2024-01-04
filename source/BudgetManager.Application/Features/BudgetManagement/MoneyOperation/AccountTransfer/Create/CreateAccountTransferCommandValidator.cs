namespace BudgetManager.Application.Features.BudgetManagement;

using BudgetManager.Application.Extensions;
using BudgetManager.Infrastructure;

public class CreateAccountTransferCommandValidator
  : BudgetCommandValidator<CreateAccountTransferCommand>
{
  public CreateAccountTransferCommandValidator(IUserBudgetRepository repository, AppConfig appConfig) : base(repository)
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
      .Must((command, cancellation) => !string.IsNullOrEmpty(command.AccountId))
        .WithMessage("Source id must be defined.")
      .Must((command, cancellation) => !string.IsNullOrEmpty(command.TargetAccountId))
        .WithMessage("Target id must be defined.")
      .DependentRules(() =>
      {
        RuleFor(x => x)
          .MustAsync(async (command, cancellation) =>
          {
            var budget = await repository.Get(command.UserId);
            return budget!.Accounts?.Any(x => x.Id == command.AccountId) ?? false;
          })
          .WithMessage(command => $"Source Account with id '{command.AccountId}' does not exist in the budget.")
          .DependentRules(() =>
          {
            RuleFor(x => x)
              .MustAsync(async (command, cancellation) =>
              {
                var budget = await repository.Get(command.UserId);
                var account = budget!.Accounts!.First(x => x.Id == command.AccountId);
                return account!.Balance!.ContainsKey(command.Value.Currency) && account!.Balance?[command.Value.Currency] >= command.Value.Amount;
              })
              .WithMessage("Insufficient funds.");
          });

        RuleFor(x => x)
          .MustAsync(async (command, cancellation) =>
          {
            var budget = await repository.Get(command.UserId);
            return budget!.Accounts?.Any(x => x.Id == command.TargetAccountId) ?? false;
          }).WithMessage(command => $"Target account with id '{command.TargetAccountId}' does not exist in the budget.");
      });
  }
}
