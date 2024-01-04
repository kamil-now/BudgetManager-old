namespace BudgetManager.Application.Features.BudgetManagement;

using AutoMapper;

public class BudgetSummaryRequestHandler(IUserBudgetRepository _repository, IMapper _mapper) : IRequestHandler<BudgetSummaryRequest, BudgetSummaryDto>
{
  public async Task<BudgetSummaryDto> Handle(BudgetSummaryRequest request, CancellationToken cancellationToken)
  {
    var budget = await _repository.Get(request.UserId);
    return _mapper.Map<BudgetSummaryDto>(budget);
  }
}
