namespace BudgetManager.Application.Features.BudgetManagement;

using BudgetManager.Domain.Models;

public record DeleteMoneyOperationCommand<T>(string UserId, string Id)
  : IRequest<T>, IMoneyOperationCommand where T : MoneyOperation;
