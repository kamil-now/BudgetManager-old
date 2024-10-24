namespace BudgetManager.Application.Features.BudgetManagement;

using AutoMapper;
using BudgetManager.Domain.Models;
using BudgetManager.Infrastructure;

public class DeleteExpenseCommandHandler(IUserBudgetRepository repo, IMapper map)
  : DeleteMoneyOperationCommandHandler<Expense>(repo, map)
{
}
