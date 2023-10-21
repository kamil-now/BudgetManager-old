namespace BudgetManager.Application.Features.BudgetManagement;

using BudgetManager.Domain.Models;

public record UpdateAllocationCommand(
  string UserId,
  string OperationId,
  string? Title,
  Money? Value,
  string? Date,
  string? TargetFundId,
  string? Description
  ) : UpdateMoneyOperationCommand<Allocation, AllocationDto>(UserId, OperationId);
