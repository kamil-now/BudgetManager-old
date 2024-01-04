namespace BudgetManager.Application.Features.BudgetManagement;

using AutoMapper;
using BudgetManager.Domain.Models;
using BudgetManager.Infrastructure;

public class UpdateUserSettingsCommandHandler(IUserBudgetRepository repo, IMapper map) 
  : BudgetCommandHandler<UpdateUserSettingsCommand, Unit>(repo, map)
{
  public override Unit ModifyBudget(UpdateUserSettingsCommand command, Budget budget)
  {
    budget.UpdateUserSettings(command.AccountsOrder, command.FundsOrder);

    return Unit.Value;
  }
}
