namespace BudgetManager.Application.Features.BudgetManagement;

using AutoMapper;
using BudgetManager.Domain.Models;
using BudgetManager.Infrastructure;

public class DeleteExpenseCommandHandler : DeleteMOneyOperationCommandHandler<Expense>
{
  public DeleteExpenseCommandHandler(IUserBudgetRepository repo, IMapper map) : base(repo, map)
  {
  }
}
