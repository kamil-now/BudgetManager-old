namespace BudgetManager.Application.Commands;

using AutoMapper;
using BudgetManager.Domain.Models;
using BudgetManager.Infrastructure;

public class DeleteCurrencyExchangeCommandHandler : DeleteOperationCommandHandler<CurrencyExchange>
{
  public DeleteCurrencyExchangeCommandHandler(IUserBudgetRepository repo, IMapper map) : base(repo, map)
  {
  }
}

public class DeleteCurrencyExchangeCommandValidator : UpdateOperationCommandValidator<DeleteOperationCommand<CurrencyExchange>>
{
  public DeleteCurrencyExchangeCommandValidator(IUserBudgetRepository repository) : base(repository)
  {
  }
}