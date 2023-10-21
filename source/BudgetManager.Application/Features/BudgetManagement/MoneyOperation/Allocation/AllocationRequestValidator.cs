namespace BudgetManager.Application.Features.BudgetManagement;

public class AllocationRequestValidator : BudgetRequestValidator<AllocationRequest>
{
  public AllocationRequestValidator(IUserBudgetRepository repository) : base(repository)
  {
    RuleFor(x => x)
      .MustAsync(async (request, cancellation) =>
      {
        var budget = await repository.Get(request.UserId);
        return budget!.Allocations?.Any(x => x.Id == request.AllocationId) ?? false;
      }).WithMessage("Allocation with a given id does not exist in the budget.");
  }
}
