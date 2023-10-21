namespace BudgetManager.Application.Features.BudgetManagement;

using BudgetManager.Domain.Models;

public record UpdateAccountTransferCommand(
  string UserId,
  string OperationId,
  string Title,
  Money Value,
  string? Date,
  string? Description,
  string? AccountId,
  string? TargetAccountId
  ) : UpdateMoneyOperationCommand<AccountTransfer, AccountTransferDto>(UserId, OperationId);
