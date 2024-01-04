namespace BudgetManager.Application.Features.BudgetManagement;

using BudgetManager.Domain.Models;
using BudgetManager.Infrastructure;

public class DeleteIncomeCommandValidator(IUserBudgetRepository repository) 
  : UpdateMoneyOperationCommandValidator<DeleteMoneyOperationCommand<Income>>(repository)
{
}
