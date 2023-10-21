namespace BudgetManager.Application.Features.BudgetManagement;

using BudgetManager.Domain.Models;
using BudgetManager.Infrastructure;

public class DeleteAllocationCommandValidator : UpdateMoneyOperationCommandValidator<DeleteMoneyOperationCommand<Allocation>>
{
  public DeleteAllocationCommandValidator(IUserBudgetRepository repository) : base(repository)
  {
  }
}
