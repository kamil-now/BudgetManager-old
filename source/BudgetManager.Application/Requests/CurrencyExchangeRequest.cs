namespace BudgetManager.Application.Requests;

using AutoMapper;

public record CurrencyExchangeRequest(string UserId, string CurrencyExchangeId) : IBudgetRequest, IRequest<CurrencyExchangeDto>;

public class CurrencyExchangeRequestHandler : BudgetRequestHandler<CurrencyExchangeRequest, CurrencyExchangeDto>
{
  public CurrencyExchangeRequestHandler(IUserBudgetRepository repo, IMapper map)
   : base(repo, map)
  {
  }

  public override CurrencyExchangeDto Get(CurrencyExchangeRequest request, Budget budget)
  {
    var currencyExchange = budget.Operations.First(x => x.Id == request.CurrencyExchangeId) as CurrencyExchange;
    return _mapper.Map<CurrencyExchangeDto>(currencyExchange);
  }
}

public class CurrencyExchangesRequestHandler : BudgetRequestHandler<BudgetRequest<CurrencyExchangeDto>, IEnumerable<CurrencyExchangeDto>>
{
  public CurrencyExchangesRequestHandler(IUserBudgetRepository repo, IMapper map)
   : base(repo, map)
  {
  }

  public override IEnumerable<CurrencyExchangeDto> Get(BudgetRequest<CurrencyExchangeDto> request, Budget budget)
   => budget.Operations.Where(x => x is CurrencyExchange).Select(x => _mapper.Map<CurrencyExchangeDto>(x as CurrencyExchange));
}

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
