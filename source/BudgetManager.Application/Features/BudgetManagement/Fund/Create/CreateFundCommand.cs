namespace BudgetManager.Application.Features.BudgetManagement;

using System.Text.Json.Serialization;

public record CreateFundCommand([property: JsonIgnore()] string UserId, string Name)
  : IRequest<string>, IBudgetCommand;
