namespace BudgetManager.Application.Features.BudgetManagement;

using System.Text.Json.Serialization;

public record CreateBudgetCommand([property: JsonIgnore()] string UserId) : IRequest<bool>;
