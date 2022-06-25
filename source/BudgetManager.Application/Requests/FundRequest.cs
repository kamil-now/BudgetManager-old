namespace BudgetManager.Application.Requests;

using AutoMapper;

public record FundDto(string Id, string Name, Money Balance);
public record FundRequest(string UserId, string FundId) : IBudgetRequest, IRequest<FundDto>;

public class FundRequestHandler : BudgetRequestHandler<FundRequest, FundDto>
{
  public FundRequestHandler(IUserBudgetRepository repo, IMapper map)
   : base(repo, map)
  {
  }

  public override FundDto Get(FundRequest request, Budget budget)
  {
    var Fund = budget.Funds.First(x => x.Id == request.FundId);
    return _mapper.Map<FundDto>(Fund);
  }
}
