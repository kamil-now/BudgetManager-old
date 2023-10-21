namespace BudgetManager.Application.Features.BudgetManagement;

public class CreateBudgetCommandValidator : AbstractValidator<CreateBudgetCommand>
{
  public CreateBudgetCommandValidator()
  {
    RuleFor(x => x.UserId)
      .NotEmpty();
  }
}
