namespace BudgetManager.Application.Features.BudgetManagement;

using AutoMapper;

public class AccountTransferRequestHandler(IUserBudgetRepository repo, IMapper map)
  : BudgetRequestHandler<AccountTransferRequest, AccountTransferDto>(repo, map), IRequestHandler<AccountTransferRequest, AccountTransferDto>
{
  public override AccountTransferDto Get(AccountTransferRequest request, Budget budget)
  {
    if (budget.Operations.First(x => x.Id == request.AccountTransferId) is not AccountTransfer accountTransfer)
    {
      throw new Exception();
    }
    return _mapper.Map<AccountTransferDto>(accountTransfer) with
    {
      Type = MoneyOperationType.AccountTransfer,
      AccountName = budget.Accounts.First(x => x.Id == accountTransfer.SourceAccountId).Name,
      TargetAccountName = budget.Accounts.First(x => x.Id == accountTransfer.TargetAccountId).Name,
    };
  }
}
