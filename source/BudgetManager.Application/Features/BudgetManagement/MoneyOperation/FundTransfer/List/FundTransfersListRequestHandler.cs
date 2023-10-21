namespace BudgetManager.Application.Features.BudgetManagement;

using AutoMapper;

public class FundTransfersListRequestHandler : BudgetRequestHandler<BudgetRequest<FundTransferDto>, IEnumerable<FundTransferDto>>
{
  public FundTransfersListRequestHandler(IUserBudgetRepository repo, IMapper map)
   : base(repo, map)
  {
  }

  public override IEnumerable<FundTransferDto> Get(BudgetRequest<FundTransferDto> request, Budget budget)
   => budget.Operations.Where(x => x is FundTransfer).Select(x => _mapper.Map<FundTransferDto>(x as FundTransfer));
}
