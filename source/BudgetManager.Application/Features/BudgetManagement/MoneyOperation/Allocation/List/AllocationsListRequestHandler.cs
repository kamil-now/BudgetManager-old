namespace BudgetManager.Application.Features.BudgetManagement;

using AutoMapper;

public class AllocationsListRequestHandler : BudgetRequestHandler<BudgetRequest<AllocationDto>, IEnumerable<AllocationDto>>
{
  public AllocationsListRequestHandler(IUserBudgetRepository repo, IMapper map)
   : base(repo, map)
  {
  }

  public override IEnumerable<AllocationDto> Get(BudgetRequest<AllocationDto> request, Budget budget) 
    => budget.Operations.Where(x => x is Allocation).Select(x => _mapper.Map<AllocationDto>(x as Allocation));
}
