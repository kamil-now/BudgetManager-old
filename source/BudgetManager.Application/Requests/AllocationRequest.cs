namespace BudgetManager.Application.Requests;

using AutoMapper;

public record AllocationRequest(string UserId, string AllocationId) : IBudgetRequest, IRequest<AllocationDto>;

public class AllocationRequestHandler : BudgetRequestHandler<AllocationRequest, AllocationDto>
{
  public AllocationRequestHandler(IUserBudgetRepository repo, IMapper map)
   : base(repo, map)
  {
  }

  public override AllocationDto Get(AllocationRequest request, Budget budget)
  {
    var Allocation = budget.Operations.First(x => x.Id == request.AllocationId) as Allocation;
    return _mapper.Map<AllocationDto>(Allocation);
  }
}

public class AllocationsRequestHandler : BudgetRequestHandler<BudgetRequest<AllocationDto>, IEnumerable<AllocationDto>>
{
  public AllocationsRequestHandler(IUserBudgetRepository repo, IMapper map)
   : base(repo, map)
  {
  }

  public override IEnumerable<AllocationDto> Get(BudgetRequest<AllocationDto> request, Budget budget)
   => budget.Operations.Where(x => x is Allocation).Select(x => _mapper.Map<AllocationDto>(x as Allocation));
}

public class AllocationRequestValidator : BudgetRequestValidator<AllocationRequest>
{
  public AllocationRequestValidator(IUserBudgetRepository repository) : base(repository)
  {
    RuleFor(x => x)
      .MustAsync(async (request, cancellation) =>
      {
        var budget = await repository.Get(request.UserId);
        return budget!.Allocations?.Any(x => x.Id == request.AllocationId) ?? false;
      }).WithMessage("Allocation with a given id does not exist in the budget.");
  }
}
