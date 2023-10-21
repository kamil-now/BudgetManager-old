namespace BudgetManager.Application.Features.BudgetManagement;

using AutoMapper;
using BudgetManager.Domain.Models;
using BudgetManager.Infrastructure;

public class DeleteCurrencyExchangeCommandHandler : DeleteMOneyOperationCommandHandler<CurrencyExchange>
{
  public DeleteCurrencyExchangeCommandHandler(IUserBudgetRepository repo, IMapper map) : base(repo, map)
  {
  }
}
