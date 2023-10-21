namespace BudgetManager.Application.Features.BudgetManagement;

using BudgetManager.Domain.Models;

public record UpdateFundTransferCommand(
  string UserId,
  string OperationId,
  string Title,
  Money Value,
  string? Date,
  string? Description,
  string? FundId,
  string? TargetFundId
  ) : UpdateMoneyOperationCommand<FundTransfer, FundTransferDto>(UserId, OperationId);
