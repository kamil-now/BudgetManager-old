namespace BudgetManager.Application.Features.BudgetManagement;

using BudgetManager.Domain.Models;
using BudgetManager.Infrastructure;

public class DeleteAllocationCommandValidator(IUserBudgetRepository repository)
  : UpdateMoneyOperationCommandValidator<DeleteMoneyOperationCommand<Allocation>>(repository)
{
}
