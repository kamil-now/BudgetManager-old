namespace BudgetManager.Application.Requests;

using AutoMapper;

public record BudgetSummaryRequest(string UserId) : IBudgetRequest, IRequest<BudgetSummaryDto>;

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

public class BudgetSummaryRequestValidator : AbstractValidator<BudgetSummaryRequest>
{
  public BudgetSummaryRequestValidator(IUserBudgetRepository repository)
  {
    RuleFor(x => x.UserId)
      .MustAsync(async (id, cancellation) => await repository.Exists(id))
        .WithMessage("Budget does not exist.");
  }
}
