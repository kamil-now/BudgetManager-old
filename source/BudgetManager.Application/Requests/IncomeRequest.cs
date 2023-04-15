namespace BudgetManager.Application.Requests;

using AutoMapper;

public record IncomeDto(
    string Id,
    string Title,
    Money Value,
    string Date,
    string AccountId,
    string FundId,
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

public class IncomeRequestValidator : BudgetRequestValidator<IncomeRequest>
{
  public IncomeRequestValidator(IUserBudgetRepository repository) : base(repository)
  {
    RuleFor(x => x)
      .MustAsync(async (request, cancellation) =>
      {
        var budget = await repository.Get(request.UserId);
        return budget!.Incomes?.Any(x => x.Id == request.IncomeId) ?? false;
      }).WithMessage("Income with a given id does not exist in the budget");
  }
}
