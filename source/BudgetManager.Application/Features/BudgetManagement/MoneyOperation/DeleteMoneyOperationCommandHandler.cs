namespace BudgetManager.Application.Features.BudgetManagement;

using AutoMapper;
using BudgetManager.Domain.Models;
using BudgetManager.Infrastructure;

public abstract class DeleteMOneyOperationCommandHandler<T> 
  : BudgetCommandHandler<DeleteMoneyOperationCommand<T>, T> where T : MoneyOperation
{
  protected DeleteMOneyOperationCommandHandler(IUserBudgetRepository repo, IMapper map)
  : base(repo, map)
  {
  }

  public override T ModifyBudget(DeleteMoneyOperationCommand<T> command, Budget budget)
    => budget.RemoveOperation<T>(command.Id);
}
