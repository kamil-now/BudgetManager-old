namespace BudgetManager.Application.Features.BudgetManagement;

using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using BudgetManager.Infrastructure;

public abstract class BudgetCommandHandler<TCommand, TResult>(
  IUserBudgetRepository _repository,
  IMapper mapper
    )
  : IRequestHandler<TCommand, TResult> where TCommand : IRequest<TResult>, IBudgetCommand
{
  protected IMapper _mapper = mapper;

  public abstract TResult ModifyBudget(TCommand command, Budget budget);

  public async Task<TResult> Handle(TCommand command, CancellationToken cancellationToken)
  {
    return await Run(command.UserId, async () =>
    {
      var src = await _repository.Get(command.UserId);
      var budget = _mapper.Map<Budget>(src);

      var id = ModifyBudget(command, budget);
      var updatedBudget = _mapper.Map<BudgetEntity>(budget);
      updatedBudget.UserId = command.UserId;

      await _repository.Update(updatedBudget);

      return id;
    });

  }
  private async Task<T> Run<T>(string userId, Func<Task<T>> task)
  {
    // TODO timeout
    bool lockAcquired = false;

    while (!lockAcquired)
    {
      lockAcquired = await _repository.TryAcquireLock(userId);
      if (!lockAcquired)
      {
        await Task.Delay(10);
      }
    }

    try
    {
      return await task();
    }
    finally
    {
      await _repository.ReleaseLock(userId);
    }
  }
}
