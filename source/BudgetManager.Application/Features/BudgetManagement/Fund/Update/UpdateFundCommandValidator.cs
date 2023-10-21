namespace BudgetManager.Application.Features.BudgetManagement;

using BudgetManager.Infrastructure;

public class UpdateFundCommandValidator : BudgetCommandValidator<UpdateFundCommand>
{
  public UpdateFundCommandValidator(IUserBudgetRepository repository) : base(repository)
  {
    RuleFor(x => x)
      .MustAsync(async (command, cancellation) =>
      {
        var budget = await repository.Get(command.UserId);
        return budget!.Funds?.Any(x => x.Id == command.FundId) ?? false;
      }).WithMessage("Fund with a given id does not exist in the budget");

    RuleFor(x => x.Name)
      .NotEmpty()
      .MaximumLength(50);
  }
}
