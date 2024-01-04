namespace BudgetManager.Application.Features.BudgetManagement;

using AutoMapper;

public class AccountRequestHandler(IUserBudgetRepository repo, IMapper map) : BudgetRequestHandler<AccountRequest, AccountDto>(repo, map)
{
  public override AccountDto Get(AccountRequest request, Budget budget)
  {
    var account = budget.Accounts.First(x => x.Id == request.AccountId);
    return _mapper.Map<AccountDto>(account);
  }
}
