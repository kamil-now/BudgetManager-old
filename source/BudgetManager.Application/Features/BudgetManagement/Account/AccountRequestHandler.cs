namespace BudgetManager.Application.Features.BudgetManagement;

using AutoMapper;

public class AccountRequestHandler : BudgetRequestHandler<AccountRequest, AccountDto>
{
  public AccountRequestHandler(IUserBudgetRepository repo, IMapper map)
   : base(repo, map)
  {
  }

  public override AccountDto Get(AccountRequest request, Budget budget)
  {
    var account = budget.Accounts.First(x => x.Id == request.AccountId);
    return _mapper.Map<AccountDto>(account);
  }
}
