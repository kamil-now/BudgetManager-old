namespace BudgetManager.Application.Features.BudgetManagement;

using System.Text.Json.Serialization;

public record UpdateUserSettingsCommand(
  [property: JsonIgnore()]
  string UserId,
  IEnumerable<string> AccountsOrder, 
  IEnumerable<string> FundsOrder) : IRequest<Unit>, IBudgetCommand;
