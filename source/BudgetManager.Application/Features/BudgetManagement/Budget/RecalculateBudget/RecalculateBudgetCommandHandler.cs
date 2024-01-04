namespace BudgetManager.Application.Features.BudgetManagement;

using AutoMapper;
using BudgetManager.Domain.Models;
using BudgetManager.Infrastructure;

public class RecalculateBudgetCommandHandler(IUserBudgetRepository repo, IMapper map) 
  : BudgetCommandHandler<RecalculateBudgetCommand, string>(repo, map)
{
  public override string ModifyBudget(RecalculateBudgetCommand command, Budget budget)
  {
    budget.Recalculate();
    return "OK";
  }
}
