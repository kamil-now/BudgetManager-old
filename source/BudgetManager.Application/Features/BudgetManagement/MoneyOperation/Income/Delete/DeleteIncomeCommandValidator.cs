namespace BudgetManager.Application.Features.BudgetManagement;

using BudgetManager.Domain.Models;
using BudgetManager.Infrastructure;

public class DeleteIncomeCommandValidator : UpdateMoneyOperationCommandValidator<DeleteMoneyOperationCommand<Income>>
{
  public DeleteIncomeCommandValidator(IUserBudgetRepository repository) : base(repository)
  {
  }
}
