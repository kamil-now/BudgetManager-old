namespace BudgetManager.Application.Features.BudgetManagement;

using System.Text.Json.Serialization;

public record RecalculateBudgetCommand([property: JsonIgnore()] string UserId) : IRequest<string>, IBudgetCommand;
