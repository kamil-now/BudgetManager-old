namespace BudgetManager.Application.Features.BudgetManagement;

using AutoMapper;

public class FundsListRequestHandler(IUserBudgetRepository repo, IMapper map) 
  : BudgetRequestHandler<BudgetRequest<FundDto>, IEnumerable<FundDto>>(repo, map)
{
  public override IEnumerable<FundDto> Get(BudgetRequest<FundDto> request, Budget budget)
  => budget.Funds.Where(x => !x.IsDeleted).Select(x => _mapper.Map<FundDto>(x));
}
