namespace BudgetManager.Application.Requests;

using AutoMapper;

public record FundRequest(string UserId, string FundId) : IBudgetRequest, IRequest<FundDto>;

public class FundRequestHandler : BudgetRequestHandler<FundRequest, FundDto>
{
  public FundRequestHandler(IUserBudgetRepository repo, IMapper map)
   : base(repo, map)
  {
  }

  public override FundDto Get(FundRequest request, Budget budget)
  {
    var fund = budget.Funds.First(x => x.Id == request.FundId);
    var x = _mapper.Map<FundDto>(fund);
    return x;
  }
}


public class FundsRequestHandler : BudgetRequestHandler<BudgetRequest<FundDto>, IEnumerable<FundDto>>
{
  public FundsRequestHandler(IUserBudgetRepository repo, IMapper map)
   : base(repo, map)
  {
  }

  public override IEnumerable<FundDto> Get(BudgetRequest<FundDto> request, Budget budget)
   => budget.Funds.Select(x => _mapper.Map<FundDto>(x));
}

public class FundRequestValidator : BudgetRequestValidator<FundRequest>
{
  public FundRequestValidator(IUserBudgetRepository repository) : base(repository)
  {
    RuleFor(x => x)
      .MustAsync(async (request, cancellation) =>
      {
        var budget = await repository.Get(request.UserId);
        return budget!.Funds?.Any(x => x.Id == request.FundId) ?? false;
      }).WithMessage("Fund with a given id does not exist in the budget");
  }
}
