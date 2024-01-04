namespace BudgetManager.Application.Features.BudgetManagement;

using AutoMapper;

public class AllocationRequestHandler(IUserBudgetRepository repo, IMapper map)
  : BudgetRequestHandler<AllocationRequest, AllocationDto>(repo, map)
{
  public override AllocationDto Get(AllocationRequest request, Budget budget)
  {
    if (budget.Operations.First(x => x.Id == request.AllocationId) is not Allocation allocation)
    {
      throw new Exception();
    }
    return _mapper.Map<AllocationDto>(allocation) with
    {
      Type = MoneyOperationType.Allocation,
      TargetFundName = budget.Funds.First(x => x.Id == allocation.TargetFundId).Name,
    };
  }
}
