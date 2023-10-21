namespace BudgetManager.Application.Features.BudgetManagement;

public class DeleteBudgetCommandValidator : AbstractValidator<DeleteBudgetCommand>
{
  public DeleteBudgetCommandValidator(IUserBudgetRepository repository)
  {
    RuleFor(x => x.UserId)
      .NotEmpty();

    RuleFor(x => x.UserId)
      .MustAsync(async (id, cancellation) => await repository.Exists(id))
      .WithMessage("Budget does not exist");
  }
}
