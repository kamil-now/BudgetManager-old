namespace BudgetManager.Application.Features.BudgetManagement;

using AutoMapper;
using BudgetManager.Domain.Models;
using BudgetManager.Infrastructure;

public class CreateFundCommandHandler : BudgetCommandHandler<CreateFundCommand, string>
{
  public CreateFundCommandHandler(IUserBudgetRepository repo, IMapper map)
  : base(repo, map)
  {
  }

  public override string ModifyBudget(CreateFundCommand command, Budget budget) 
    => budget.AddFund(command.Name);
}
