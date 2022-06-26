namespace BudgetManager.Application.Commands;

using AutoMapper;
using BudgetManager.Domain.Models;
using BudgetManager.Infrastructure;

public interface IOperationCommand : IBudgetCommand
{
  string OperationId { get; init; }
}

public record DeleteOperationCommand<T>(string UserId, string OperationId)
  : IRequest<Unit>, IOperationCommand where T : MoneyOperation;

public abstract class DeleteOperationCommandHandler<T>
  : BudgetCommandHandler<DeleteOperationCommand<T>, Unit> where T : MoneyOperation
{
  protected DeleteOperationCommandHandler(IUserBudgetRepository repo, IMapper map)
  : base(repo, map)
  {
  }

  public override Unit ModifyBudget(DeleteOperationCommand<T> command, Budget budget)
  {
    budget.RemoveOperation<T>(command.OperationId);
    return Unit.Value;
  }
}
