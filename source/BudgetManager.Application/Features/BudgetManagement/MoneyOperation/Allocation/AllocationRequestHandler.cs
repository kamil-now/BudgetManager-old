namespace BudgetManager.Application.Features.BudgetManagement;

using AutoMapper;

public class AllocationRequestHandler : BudgetRequestHandler<AllocationRequest, AllocationDto>
{
  public AllocationRequestHandler(IUserBudgetRepository repo, IMapper map)
   : base(repo, map)
  {
  }

  public override AllocationDto Get(AllocationRequest request, Budget budget)
  {
    var allocation = budget.Operations.First(x => x.Id == request.AllocationId) as Allocation;
    if (allocation is null)
    {
      throw new Exception();
    }
    return _mapper.Map<AllocationDto>(allocation)
     with
    {
      Type = MoneyOperationType.Allocation,
      TargetFundName = budget.Funds.First(x => x.Id == allocation.TargetFundId).Name,
    };
  }
}
