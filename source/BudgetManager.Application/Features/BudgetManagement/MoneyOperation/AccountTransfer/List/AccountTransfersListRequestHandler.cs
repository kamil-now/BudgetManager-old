namespace BudgetManager.Application.Features.BudgetManagement;

using AutoMapper;

public class AccountTransfersListRequestHandler : BudgetRequestHandler<BudgetRequest<AccountTransferDto>, IEnumerable<AccountTransferDto>>
{
  public AccountTransfersListRequestHandler(IUserBudgetRepository repo, IMapper map)
   : base(repo, map)
  {
  }

  public override IEnumerable<AccountTransferDto> Get(BudgetRequest<AccountTransferDto> request, Budget budget)
   => budget.Operations.Where(x => x is AccountTransfer).Select(x => _mapper.Map<AccountTransferDto>(x as AccountTransfer));
}
