namespace BudgetManager.Application.Features.BudgetManagement;

using BudgetManager.Infrastructure;

public class UpdateAccountTransferCommandValidator : UpdateMoneyOperationCommandValidator<UpdateAccountTransferCommand>
{
  public UpdateAccountTransferCommandValidator(IUserBudgetRepository repository, AppConfig appConfig) : base(repository)
  {
    RuleFor(x => x.Title)
      .Must(title => title is null || title.Length <= appConfig.MaxTitleLength);

    RuleFor(x => x)
      .MustAsync(async (command, cancellation) =>
      {
        if (command.AccountId is null)
          return true;
        var budget = await repository.Get(command.UserId);
        return budget!.Accounts?.Any(x => x.Id == command.AccountId) ?? false;
      })
      .WithMessage(command => $"Source Account with id {command.AccountId} does not exist in the budget");

    RuleFor(x => x)
      .MustAsync(async (command, cancellation) =>
      {
        if (command.TargetAccountId is null)
          return true;
        var budget = await repository.Get(command.UserId);
        return budget!.Accounts?.Any(x => x.Id == command.TargetAccountId) ?? false;
      }).WithMessage(command => $"Target account with id {command.TargetAccountId} does not exist in the budget");
  }
}
