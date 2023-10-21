namespace BudgetManager.Application.Features.BudgetManagement;

using BudgetManager.Domain.Models;

public record UpdateIncomeCommand(
  string UserId,
  string OperationId,
  string? Title,
  Money? Value,
  string? Date,
  string? AccountId,
  string? Description
  ) : UpdateMoneyOperationCommand<Income, IncomeDto>(UserId, OperationId);
