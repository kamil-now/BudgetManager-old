namespace BudgetManager.Application.Features.BudgetManagement;

public class ExpenseRequestValidator : BudgetRequestValidator<ExpenseRequest>
{
  public ExpenseRequestValidator(IUserBudgetRepository repository) : base(repository)
  {
    RuleFor(x => x)
      .MustAsync(async (request, cancellation) =>
      {
        var budget = await repository.Get(request.UserId);
        return budget!.Expenses?.Any(x => x.Id == request.ExpenseId) ?? false;
      }).WithMessage("Expense with a given id does not exist in the budget.");
  }
}
