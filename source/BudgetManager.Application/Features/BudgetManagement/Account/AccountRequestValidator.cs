namespace BudgetManager.Application.Features.BudgetManagement;

public class AccountRequestValidator : BudgetRequestValidator<AccountRequest>
{
  public AccountRequestValidator(IUserBudgetRepository repository) : base(repository)
  {
    RuleFor(x => x)
      .MustAsync(async (request, cancellation) =>
      {
        var budget = await repository.Get(request.UserId);
        return budget!.Accounts?.Any(x => x.Id == request.AccountId) ?? false;
      }).WithMessage("Account with a given id does not exist in the budget.");
  }
}
