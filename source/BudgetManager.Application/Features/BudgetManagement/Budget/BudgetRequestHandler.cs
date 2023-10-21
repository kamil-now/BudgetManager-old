namespace BudgetManager.Application.Features.BudgetManagement;

using AutoMapper;

public abstract class BudgetRequestHandler<TRequest, TResult> : IRequestHandler<TRequest, TResult> where TRequest : IBudgetRequest, IRequest<TResult>
{
  private IUserBudgetRepository _repository;
  protected IMapper _mapper;

  protected BudgetRequestHandler(
    IUserBudgetRepository repository,
    IMapper mapper
    )
  {
    _repository = repository;
    _mapper = mapper;
  }

  public abstract TResult Get(TRequest request, Budget budget);

  public async Task<TResult> Handle(TRequest request, CancellationToken cancellationToken)
  {
    var src = await _repository.Get(request.UserId);
    var budget = _mapper.Map<Budget>(src);

    return Get(request, budget);
  }
}
