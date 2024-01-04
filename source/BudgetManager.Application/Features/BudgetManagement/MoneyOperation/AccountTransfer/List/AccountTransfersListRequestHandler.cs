namespace BudgetManager.Application.Features.BudgetManagement;

using AutoMapper;

public class AccountTransfersListRequestHandler(IUserBudgetRepository repo, IMapper map)
  : BudgetRequestHandler<BudgetRequest<AccountTransferDto>, IEnumerable<AccountTransferDto>>(repo, map)
{
  public override IEnumerable<AccountTransferDto> Get(BudgetRequest<AccountTransferDto> request, Budget budget)
  => budget.Operations.Where(x => x is AccountTransfer).Select(x => _mapper.Map<AccountTransferDto>(x as AccountTransfer));
}
