namespace BudgetManager.Application.Features.BudgetManagement;

using System.Text.Json.Serialization;
using BudgetManager.Domain.Models;

public record UpdateMoneyOperationCommand<T, TDto>([property: JsonIgnore()] string UserId, string Id)
  : IRequest<TDto>, IMoneyOperationCommand where T : MoneyOperation;
