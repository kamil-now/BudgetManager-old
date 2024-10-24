namespace BudgetManager.Application.Features.BudgetManagement;

using AutoMapper;
using BudgetManager.Domain.Models;
using BudgetManager.Infrastructure;

public abstract class DeleteMoneyOperationCommandHandler<T>(IUserBudgetRepository repo, IMapper map)
  : BudgetCommandHandler<DeleteMoneyOperationCommand<T>, T>(repo, map) where T : MoneyOperation
{
  public override T ModifyBudget(DeleteMoneyOperationCommand<T> command, Budget budget)
  => budget.RemoveOperation<T>(command.Id);
}
