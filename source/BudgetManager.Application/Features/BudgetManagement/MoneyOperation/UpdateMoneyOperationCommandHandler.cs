namespace BudgetManager.Application.Features.BudgetManagement;

using AutoMapper;
using BudgetManager.Domain.Models;
using BudgetManager.Infrastructure;

public abstract class UpdateMoneyOperationCommandHandler<TCommand, T, TDto>(IUserBudgetRepository repo, IMapper map)
  : BudgetCommandHandler<TCommand, TDto>(repo, map) where T : MoneyOperation where TCommand : UpdateMoneyOperationCommand<T, TDto>
{
  public override TDto ModifyBudget(TCommand command, Budget budget)
  {
    var operation = budget.UpdateOperation<T>(command.Id, o => Update(o, command));
    var dto = _mapper.Map<TDto>(operation);
    return CompleteDto(dto, budget);
  }

  protected abstract void Update(T operation, TCommand command);

  protected abstract TDto CompleteDto(TDto dto, Budget budget);
}
