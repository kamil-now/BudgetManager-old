namespace BudgetManager.Application.Features.BudgetManagement;

using AutoMapper;
using BudgetManager.Domain.Models;
using BudgetManager.Infrastructure;

public class CreateFundCommandHandler(IUserBudgetRepository repo, IMapper map)
  : BudgetCommandHandler<CreateFundCommand, string>(repo, map)
{
  public override string ModifyBudget(CreateFundCommand command, Budget budget)
  => budget.AddFund(command.Name);
}
