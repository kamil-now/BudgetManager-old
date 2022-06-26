namespace BudgetManager.Application.Requests;

using AutoMapper;

public interface IBudgetRequest
{
  string UserId { get; init; }
}

public record BudgetRequest<TDto>(string UserId) : IBudgetRequest, IRequest<IEnumerable<TDto>>;

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
