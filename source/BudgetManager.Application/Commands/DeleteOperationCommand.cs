namespace BudgetManager.Application.Commands;

using AutoMapper;
using BudgetManager.Domain.Models;
using BudgetManager.Infrastructure;

public record DeleteOperationCommand<T>(string UserId, string Id)
  : IRequest<T>, IOperationCommand where T : MoneyOperation;

public abstract class DeleteOperationCommandHandler<T> 
  : BudgetCommandHandler<DeleteOperationCommand<T>, T> where T : MoneyOperation
{
  protected DeleteOperationCommandHandler(IUserBudgetRepository repo, IMapper map)
  : base(repo, map)
  {
  }

  public override T ModifyBudget(DeleteOperationCommand<T> command, Budget budget)
    => budget.RemoveOperation<T>(command.Id);
}
