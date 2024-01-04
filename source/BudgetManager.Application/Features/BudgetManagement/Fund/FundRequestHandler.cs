namespace BudgetManager.Application.Features.BudgetManagement;

using AutoMapper;

public class FundRequestHandler(IUserBudgetRepository repo, IMapper map) : BudgetRequestHandler<FundRequest, FundDto>(repo, map)
{
  public override FundDto Get(FundRequest request, Budget budget)
  {
    var fund = budget.Funds.First(x => x.Id == request.FundId);
    var x = _mapper.Map<FundDto>(fund);
    return x;
  }
}
