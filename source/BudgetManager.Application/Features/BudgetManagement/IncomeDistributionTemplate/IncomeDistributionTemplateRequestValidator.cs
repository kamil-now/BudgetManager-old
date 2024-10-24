namespace BudgetManager.Application.Features.BudgetManagement;

public class IncomeDistributionTemplateRequestValidator : BudgetRequestValidator<IncomeDistributionTemplateRequest>
{
  public IncomeDistributionTemplateRequestValidator(IUserBudgetRepository repository) : base(repository)
  {
    RuleFor(x => x)
      .MustAsync(async (request, cancellation) =>
      {
        var budget = await repository.Get(request.UserId);
        return budget!.IncomeDistributionTemplates?.Any(x => x.Id == request.IncomeDistributionTemplateId) ?? false;
      }).WithMessage("IncomeDistributionTemplate with a given id does not exist in the budget.");
  }
}
