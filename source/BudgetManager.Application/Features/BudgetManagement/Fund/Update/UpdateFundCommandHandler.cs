namespace BudgetManager.Application.Features.BudgetManagement;

using AutoMapper;
using BudgetManager.Domain.Models;
using BudgetManager.Infrastructure;

public class UpdateFundCommandHandler(IUserBudgetRepository repo, IMapper map) : BudgetCommandHandler<UpdateFundCommand, FundDto>(repo, map)
{
  public override FundDto ModifyBudget(UpdateFundCommand command, Budget budget)
  => _mapper.Map<FundDto>(budget.RenameFund(command.FundId, command.Name));
}
