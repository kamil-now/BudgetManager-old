namespace BudgetManager.Application.Features.BudgetManagement;

using BudgetManager.Domain.Models;
using BudgetManager.Infrastructure;

public class DeleteAccountTransferCommandValidator : UpdateMoneyOperationCommandValidator<DeleteMoneyOperationCommand<AccountTransfer>>
{
  public DeleteAccountTransferCommandValidator(IUserBudgetRepository repository) : base(repository)
  {
  }
}
