namespace BudgetManager.Application.Features.BudgetManagement;

using AutoMapper;
using BudgetManager.Domain.Models;
using BudgetManager.Infrastructure;

public class CreateAccountCommandHandler(IUserBudgetRepository repo, IMapper map) : BudgetCommandHandler<CreateAccountCommand, string>(repo, map)
{
  public override string ModifyBudget(CreateAccountCommand command, Budget budget)
    => budget.AddAccount(command.Name, command.InitialBalance);
}
