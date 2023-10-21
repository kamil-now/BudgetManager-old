namespace BudgetManager.Application.Features.BudgetManagement;

using System.Text.Json.Serialization;
using BudgetManager.Domain.Models;

public record CreateAccountCommand([property: JsonIgnore()] string UserId, string Name, Balance InitialBalance) : IRequest<string>, IBudgetCommand;
