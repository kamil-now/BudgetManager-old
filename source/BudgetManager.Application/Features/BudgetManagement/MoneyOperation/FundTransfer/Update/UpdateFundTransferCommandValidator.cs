namespace BudgetManager.Application.Features.BudgetManagement;

using BudgetManager.Infrastructure;

public class UpdateFundTransferCommandValidator : UpdateMoneyOperationCommandValidator<UpdateFundTransferCommand>
{
  public UpdateFundTransferCommandValidator(IUserBudgetRepository repository, AppConfig appConfig) : base(repository)
  {
    RuleFor(x => x.Title)
      .Must(title => title is null || title.Length <= appConfig.MaxTitleLength);

    RuleFor(x => x)
      .MustAsync(async (command, cancellation) =>
      {
        if (command.FundId is null)
          return true;
        var budget = await repository.Get(command.UserId);
        return budget!.Funds?.Any(x => x.Id == command.FundId) ?? false;
      })
      .WithMessage(command => $"Source Fund with id {command.FundId} does not exist in the budget");

    RuleFor(x => x)
      .MustAsync(async (command, cancellation) =>
      {
        if (command.TargetFundId is null)
          return true;
        var budget = await repository.Get(command.UserId);
        return budget!.Funds?.Any(x => x.Id == command.TargetFundId) ?? false;
      }).WithMessage(command => $"Target fund with id {command.TargetFundId} does not exist in the budget");
  }
}
