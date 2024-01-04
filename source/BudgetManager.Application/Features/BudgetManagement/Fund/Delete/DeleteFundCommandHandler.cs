namespace BudgetManager.Application.Features.BudgetManagement;

using AutoMapper;
using BudgetManager.Domain.Models;
using BudgetManager.Infrastructure;

public class DeleteFundCommandHandler(IUserBudgetRepository repo, IMapper map) : BudgetCommandHandler<DeleteFundCommand, Unit>(repo, map)
{
  public override Unit ModifyBudget(DeleteFundCommand command, Budget budget)
  {
    budget.RemoveFund(command.FundId);
    return Unit.Value;
  }
}
