namespace BudgetManager.Application.Features.BudgetManagement;

using BudgetManager.Domain.Models;
using BudgetManager.Infrastructure;

public class DeleteCurrencyExchangeCommandValidator : UpdateMoneyOperationCommandValidator<DeleteMoneyOperationCommand<CurrencyExchange>>
{
  public DeleteCurrencyExchangeCommandValidator(IUserBudgetRepository repository) : base(repository)
  {
  }
}