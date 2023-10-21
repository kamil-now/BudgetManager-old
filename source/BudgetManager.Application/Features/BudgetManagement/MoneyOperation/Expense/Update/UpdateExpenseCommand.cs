namespace BudgetManager.Application.Features.BudgetManagement;

using BudgetManager.Domain.Models;

public record UpdateExpenseCommand(
  string UserId,
  string OperationId,
  string? Title,
  Money? Value,
  string? Date,
  string? AccountId,
  string? FundId,
  string? Description
  ) : UpdateMoneyOperationCommand<Expense, ExpenseDto>(UserId, OperationId);
