namespace BudgetManager.Application.Features.BudgetManagement;

using AutoMapper;

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
