namespace BudgetManager.Application.Features.BudgetManagement;

using AutoMapper;
using BudgetManager.Domain.Models;
using BudgetManager.Infrastructure;

public class UpdateAccountCommandHandler(IUserBudgetRepository repo, IMapper map) : BudgetCommandHandler<UpdateAccountCommand, AccountDto>(repo, map)
{
  public override AccountDto ModifyBudget(UpdateAccountCommand command, Budget budget)
  => _mapper.Map<AccountDto>(budget.UpdateAccount(command.AccountId, command.Name, command.InitialBalance));
}
