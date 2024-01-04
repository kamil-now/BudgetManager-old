namespace BudgetManager.Application.Features.BudgetManagement;

using BudgetManager.Domain.Models;
using BudgetManager.Infrastructure;

public class DeleteFundTransferCommandValidator(IUserBudgetRepository repository)
  : UpdateMoneyOperationCommandValidator<DeleteMoneyOperationCommand<FundTransfer>>(repository)
{
}
