namespace BudgetManager.Application.Features.BudgetManagement;

using AutoMapper;
using BudgetManager.Domain.Models;
using BudgetManager.Infrastructure;

public class DeleteIncomeCommandHandler : DeleteMoneyOperationCommandHandler<Income>
{
  public DeleteIncomeCommandHandler(IUserBudgetRepository repo, IMapper map) : base(repo, map)
  {
  }
}
