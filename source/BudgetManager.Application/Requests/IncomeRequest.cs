namespace BudgetManager.Application.Requests;

using AutoMapper;

public record IncomeDto(
    string Id,
    string Title,
    Money Value,
    DateOnly Date,
    string AccountId,
    string Description
);
public record IncomeRequest(string UserId, string IncomeId) : IBudgetRequest, IRequest<IncomeDto>;

public class IncomeRequestHandler : BudgetRequestHandler<IncomeRequest, IncomeDto>
{
  public IncomeRequestHandler(IUserBudgetRepository repo, IMapper map)
   : base(repo, map)
  {
  }

  public override IncomeDto Get(IncomeRequest request, Budget budget)
  {
    var Income = budget.Operations.First(x => x.Id == request.IncomeId) as Income;
    return _mapper.Map<IncomeDto>(Income);
  }
}

public class IncomesRequestHandler : BudgetRequestHandler<BudgetRequest<IncomeDto>, IEnumerable<IncomeDto>>
{
  public IncomesRequestHandler(IUserBudgetRepository repo, IMapper map)
   : base(repo, map)
  {
  }

  public override IEnumerable<IncomeDto> Get(BudgetRequest<IncomeDto> request, Budget budget)
   => budget.Operations.Where(x => x is Income).Select(x => _mapper.Map<IncomeDto>(x as Income));
}
