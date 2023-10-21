namespace BudgetManager.Application.Features.BudgetManagement;

public class AccountTransferRequestValidator : BudgetRequestValidator<AccountTransferRequest>
{
  public AccountTransferRequestValidator(IUserBudgetRepository repository) : base(repository)
  {
    RuleFor(x => x)
      .MustAsync(async (request, cancellation) =>
      {
        var budget = await repository.Get(request.UserId);
        return budget!.AccountTransfers?.Any(x => x.Id == request.AccountTransferId) ?? false;
      }).WithMessage("Account transfer with a given id does not exist in the budget.");
  }
}
