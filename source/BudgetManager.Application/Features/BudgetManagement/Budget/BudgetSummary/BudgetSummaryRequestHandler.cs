namespace BudgetManager.Application.Features.BudgetManagement;

using AutoMapper;

public class BudgetSummaryRequestHandler : IRequestHandler<BudgetSummaryRequest, BudgetSummaryDto>
{
  private IUserBudgetRepository _repository;
  protected IMapper _mapper;

  public BudgetSummaryRequestHandler(
    IUserBudgetRepository repository,
    IMapper mapper
    )
  {
    _repository = repository;
    _mapper = mapper;
  }

  public async Task<BudgetSummaryDto> Handle(BudgetSummaryRequest request, CancellationToken cancellationToken)
  {
    var budget = await _repository.Get(request.UserId);
    return _mapper.Map<BudgetSummaryDto>(budget);
  }
}
