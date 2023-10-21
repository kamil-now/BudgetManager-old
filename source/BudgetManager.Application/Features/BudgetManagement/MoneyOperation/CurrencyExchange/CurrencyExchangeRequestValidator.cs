namespace BudgetManager.Application.Features.BudgetManagement;

public class CurrencyExchangeRequestValidator : BudgetRequestValidator<CurrencyExchangeRequest>
{
  public CurrencyExchangeRequestValidator(IUserBudgetRepository repository) : base(repository)
  {
    RuleFor(x => x)
      .MustAsync(async (request, cancellation) =>
      {
        var budget = await repository.Get(request.UserId);
        return budget!.CurrencyExchanges?.Any(x => x.Id == request.CurrencyExchangeId) ?? false;
      }).WithMessage("CurrencyExchange with a given id does not exist in the budget.");
  }
}
