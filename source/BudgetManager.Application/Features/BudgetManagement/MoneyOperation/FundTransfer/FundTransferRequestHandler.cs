namespace BudgetManager.Application.Features.BudgetManagement;

using AutoMapper;

public class FundTransferRequestHandler(IUserBudgetRepository repo, IMapper map)
  : BudgetRequestHandler<FundTransferRequest, FundTransferDto>(repo, map)
{
  public override FundTransferDto Get(FundTransferRequest request, Budget budget)
  {
    if (budget.Operations.First(x => x.Id == request.FundTransferId) is not FundTransfer fundTransfer)
    {
      throw new Exception();
    }
    return _mapper.Map<FundTransferDto>(fundTransfer) with
    {
      Type = MoneyOperationType.FundTransfer,
      FundName = budget.Funds.First(x => x.Id == fundTransfer.SourceFundId).Name,
      TargetFundName = budget.Funds.First(x => x.Id == fundTransfer.TargetFundId).Name,
    };
  }
}
