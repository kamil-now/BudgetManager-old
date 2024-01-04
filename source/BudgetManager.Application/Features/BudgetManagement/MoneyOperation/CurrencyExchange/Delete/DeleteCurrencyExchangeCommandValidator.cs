namespace BudgetManager.Application.Features.BudgetManagement;

using BudgetManager.Domain.Models;
using BudgetManager.Infrastructure;

public class DeleteCurrencyExchangeCommandValidator(IUserBudgetRepository repository)
  : UpdateMoneyOperationCommandValidator<DeleteMoneyOperationCommand<CurrencyExchange>>(repository)
{
}