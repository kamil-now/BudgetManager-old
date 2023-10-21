namespace BudgetManager.Application.Features.BudgetManagement;

using AutoMapper;
using BudgetManager.Domain.Models;
using BudgetManager.Infrastructure;

public class UpdateUserSettingsCommandHandler : BudgetCommandHandler<UpdateUserSettingsCommand, Unit>
{
  public UpdateUserSettingsCommandHandler(IUserBudgetRepository repo, IMapper map)
  : base(repo, map)
  {
  }

  public override Unit ModifyBudget(UpdateUserSettingsCommand command, Budget budget)
  {
    budget.UpdateUserSettings(command.AccountsOrder, command.FundsOrder);

    return Unit.Value;
  }
}
