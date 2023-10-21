namespace BudgetManager.Application.Features.BudgetManagement;

using BudgetManager.Infrastructure;

public class UpdateAllocationCommandValidator : UpdateMoneyOperationCommandValidator<UpdateAllocationCommand>
{
  public UpdateAllocationCommandValidator(IUserBudgetRepository repository, AppConfig config) : base(repository)
  {
    RuleFor(x => x.Title)
      .Must(title => title is null || title.Length <= config.MaxTitleLength);

    RuleFor(x => x.Value).ISO_4217_Currency(allowNull: true);

    RuleFor(x => x)
      .MustAsync(async (command, cancellation) =>
      {
        if (command.TargetFundId is null)
          return true;
        var budget = await repository.Get(command.UserId);
        return budget.Funds?.Any(x => x.Id == command.TargetFundId) ?? false;
      }).WithMessage(command => $"Fund with id {command.TargetFundId} does not exist in the budget");
  }
}
