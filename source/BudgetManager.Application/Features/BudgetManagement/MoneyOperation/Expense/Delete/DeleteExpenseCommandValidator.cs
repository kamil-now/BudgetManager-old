namespace BudgetManager.Application.Features.BudgetManagement;

using BudgetManager.Domain.Models;
using BudgetManager.Infrastructure;

public class DeleteExpenseCommandValidator : UpdateMoneyOperationCommandValidator<DeleteMoneyOperationCommand<Expense>>
{
  public DeleteExpenseCommandValidator(IUserBudgetRepository repository) : base(repository)
  {
  }
}