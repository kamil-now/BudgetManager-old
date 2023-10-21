namespace BudgetManager.Application.Features.BudgetManagement;

using AutoMapper;

public class AccountTransferRequestHandler : BudgetRequestHandler<AccountTransferRequest, AccountTransferDto>,IRequestHandler<AccountTransferRequest, AccountTransferDto>
{
  public AccountTransferRequestHandler(IUserBudgetRepository repo, IMapper map)
   : base(repo, map)
  {
  }

  public override AccountTransferDto Get(AccountTransferRequest request, Budget budget)
  {
    var accountTransfer = budget.Operations.First(x => x.Id == request.AccountTransferId) as AccountTransfer;
    if (accountTransfer is null)
    {
      throw new Exception();
    }
    return _mapper.Map<AccountTransferDto>(accountTransfer)
     with
    {
      Type = MoneyOperationType.AccountTransfer,
      AccountName = budget.Accounts.First(x => x.Id == accountTransfer.SourceAccountId).Name,
      TargetAccountName = budget.Accounts.First(x => x.Id == accountTransfer.TargetAccountId).Name,
    };
  }
}
