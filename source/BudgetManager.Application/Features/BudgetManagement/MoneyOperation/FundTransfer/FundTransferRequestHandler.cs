namespace BudgetManager.Application.Features.BudgetManagement;

using AutoMapper;

public class FundTransferRequestHandler : BudgetRequestHandler<FundTransferRequest, FundTransferDto>
{
  public FundTransferRequestHandler(IUserBudgetRepository repo, IMapper map)
   : base(repo, map)
  {
  }

  public override FundTransferDto Get(FundTransferRequest request, Budget budget)
  {
    var fundTransfer = budget.Operations.First(x => x.Id == request.FundTransferId) as FundTransfer;
    if(fundTransfer is null){
      throw new Exception();
    }
    return _mapper.Map<FundTransferDto>(fundTransfer)
     with
    {
      Type = MoneyOperationType.FundTransfer,
      FundName = budget.Funds.First(x => x.Id == fundTransfer.SourceFundId).Name,
      TargetFundName = budget.Funds.First(x => x.Id == fundTransfer.TargetFundId).Name,
    };
  }
}
