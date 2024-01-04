namespace BudgetManager.Application.Features.BudgetManagement;

using AutoMapper;
using BudgetManager.Domain.Models;
using BudgetManager.Infrastructure;

public class DeleteAccountCommandHandler(IUserBudgetRepository repo, IMapper map) : BudgetCommandHandler<DeleteAccountCommand, Unit>(repo, map)
{
  public override Unit ModifyBudget(DeleteAccountCommand command, Budget budget)
  {
    budget.RemoveAccount(command.AccountId);
    return Unit.Value;
  }
}
