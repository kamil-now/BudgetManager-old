namespace BudgetManager.Application.Features.BudgetManagement;

using BudgetManager.Infrastructure;

public class CreateManyAllocationsCommandValidator
  : BudgetCommandValidator<CreateManyAllocationsCommand>
{
  public CreateManyAllocationsCommandValidator(IUserBudgetRepository repository, AppConfig config) : base(repository)
  {
    RuleForEach(x => x.Allocations).ChildRules(allocation =>
    {
      allocation.RuleFor(x => x.Title)
        .MaximumLength(config.MaxTitleLength);

      allocation.RuleFor(x => x.Description)
        .MaximumLength(config.MaxContentLength);

      allocation.RuleFor(x => x.Value.Amount)
        .NotEqual(0);
    });
  }

  protected override void RulesWhenBudgetExists()
  {
    RuleForEach(x => x.Allocations)
        .MustAsync(async (command, allocation, cancellation) =>
            (await repository.Get(command.UserId)).Funds?.Any(fund => fund.Id == allocation.TargetFundId && !fund.IsDeleted) ?? false)
        .WithMessage("Target fund is deleted or does not exist.");
  }
}
