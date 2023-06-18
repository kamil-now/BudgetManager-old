namespace BudgetManager.Application.Commands;

using System.Text.Json.Serialization;
using AutoMapper;
using BudgetManager.Domain.Models;
using BudgetManager.Infrastructure;

public record UpdateUserSettingsCommand(
  [property: JsonIgnore()]
  string UserId,
  IEnumerable<string> AccountsOrder, 
  IEnumerable<string> FundsOrder) : IRequest<Unit>, IBudgetCommand;

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

public class UpdateUserSettingsCommandValidator : BudgetCommandValidator<UpdateUserSettingsCommand>
{
  public UpdateUserSettingsCommandValidator(IUserBudgetRepository repository) : base(repository)
  {
    RuleFor(x => x.AccountsOrder)
      .NotEmpty();
    RuleFor(x => x.FundsOrder)
      .NotEmpty();
  }
}