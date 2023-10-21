namespace BudgetManager.Application.Features.BudgetManagement;

public class FundTransferRequestValidator : BudgetRequestValidator<FundTransferRequest>
{
  public FundTransferRequestValidator(IUserBudgetRepository repository) : base(repository)
  {
    RuleFor(x => x)
      .MustAsync(async (request, cancellation) =>
      {
        var budget = await repository.Get(request.UserId);
        return budget!.FundTransfers?.Any(x => x.Id == request.FundTransferId) ?? false;
      }).WithMessage("Fund transfer with a given id does not exist in the budget.");
  }
}
