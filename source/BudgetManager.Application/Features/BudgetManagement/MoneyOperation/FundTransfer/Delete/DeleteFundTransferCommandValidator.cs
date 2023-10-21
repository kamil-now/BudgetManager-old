namespace BudgetManager.Application.Features.BudgetManagement;

using BudgetManager.Domain.Models;
using BudgetManager.Infrastructure;

public class DeleteFundTransferCommandValidator : UpdateMoneyOperationCommandValidator<DeleteMoneyOperationCommand<FundTransfer>>
{
  public DeleteFundTransferCommandValidator(IUserBudgetRepository repository) : base(repository)
  {
  }
}
