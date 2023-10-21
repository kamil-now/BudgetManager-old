namespace BudgetManager.Application.Features.BudgetManagement;

using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using BudgetManager.Infrastructure;

public abstract class BudgetCommandHandler<TCommand, TResult>
  : IRequestHandler<TCommand, TResult> where TCommand : IRequest<TResult>, IBudgetCommand
{
  private IUserBudgetRepository _repository;
  protected IMapper _mapper;

  protected BudgetCommandHandler(
    IUserBudgetRepository repository,
    IMapper mapper
    )
  {
    _repository = repository;
    _mapper = mapper;
  }

  public abstract TResult ModifyBudget(TCommand command, Budget budget);

  public async Task<TResult> Handle(TCommand command, CancellationToken cancellationToken)
  {
    var src = await _repository.Get(command.UserId);
    var budget = _mapper.Map<Budget>(src);

    var id = ModifyBudget(command, budget);
    var updatedBudget = _mapper.Map<BudgetEntity>(budget);
    updatedBudget.UserId = command.UserId;

    await _repository.Update(updatedBudget);

    return id;
  }
}
