namespace BudgetManager.Application.Commands;

using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using BudgetManager.Infrastructure;

public interface IBudgetCommand
{
  string UserId { get; init; }
}

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

public abstract class BudgetCommandValidator<T> : AbstractValidator<T> where T : IBudgetCommand
{
  protected IUserBudgetRepository repository;
  protected BudgetCommandValidator(IUserBudgetRepository repository)
  {
    this.repository = repository;
    RuleFor(x => x.UserId)
      .MustAsync(async (id, cancellation) => await repository.Exists(id))
      .WithMessage("Budget does not exist.")
      .DependentRules(() => RulesWhenBudgetExists());
  }

  protected virtual void RulesWhenBudgetExists() { }
}