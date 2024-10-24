namespace BudgetManager.Application.Features.BudgetManagement;

public class IncomeAllocationTemplateRequestValidator : BudgetRequestValidator<IncomeAllocationTemplateRequest>
{
  public IncomeAllocationTemplateRequestValidator(IUserBudgetRepository repository) : base(repository)
  {
    RuleFor(x => x)
      .MustAsync(async (request, cancellation) =>
      {
        var budget = await repository.Get(request.UserId);
        return budget!.IncomeAllocationTemplates?.Any(x => x.Id == request.IncomeAllocationTemplateId) ?? false;
      }).WithMessage("IncomeAllocationTemplate with a given id does not exist in the budget.");
  }
}
