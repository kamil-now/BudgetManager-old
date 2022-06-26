namespace BudgetManager.Application.Commands;

using AutoMapper;
using BudgetManager.Domain.Models;
using BudgetManager.Infrastructure;

public class DeleteExpenseCommandHandler : DeleteOperationCommandHandler<Expense>
{
  public DeleteExpenseCommandHandler(IUserBudgetRepository repo, IMapper map) : base(repo, map)
  {
  }
}

public class DeleteExpenseCommandValidator : ExpenseCommandValidator<DeleteOperationCommand<Expense>>
{
  public DeleteExpenseCommandValidator(IUserBudgetRepository repository) : base(repository)
  {
  }
}