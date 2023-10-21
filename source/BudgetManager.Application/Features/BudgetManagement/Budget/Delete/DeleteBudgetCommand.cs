namespace BudgetManager.Application.Features.BudgetManagement;

using System.Text.Json.Serialization;

public record DeleteBudgetCommand([property: JsonIgnore()] string UserId) : IRequest<Unit>;
