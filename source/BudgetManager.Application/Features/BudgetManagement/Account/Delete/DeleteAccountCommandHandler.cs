namespace BudgetManager.Application.Features.BudgetManagement;

using AutoMapper;
using BudgetManager.Domain.Models;
using BudgetManager.Infrastructure;

public class DeleteAccountCommandHandler : BudgetCommandHandler<DeleteAccountCommand, Unit>
{
  public DeleteAccountCommandHandler(IUserBudgetRepository repo, IMapper map)
  : base(repo, map)
  {
  }

  public override Unit ModifyBudget(DeleteAccountCommand command, Budget budget)
  {
    budget.RemoveAccount(command.AccountId);
    return Unit.Value;
  }
}
