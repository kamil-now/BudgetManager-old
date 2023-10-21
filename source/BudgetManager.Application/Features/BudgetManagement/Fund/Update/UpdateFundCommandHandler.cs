namespace BudgetManager.Application.Features.BudgetManagement;

using AutoMapper;
using BudgetManager.Domain.Models;
using BudgetManager.Infrastructure;

public class UpdateFundCommandHandler : BudgetCommandHandler<UpdateFundCommand, FundDto>
{
  public UpdateFundCommandHandler(IUserBudgetRepository repo, IMapper map)
  : base(repo, map)
  {
  }

  public override FundDto ModifyBudget(UpdateFundCommand command, Budget budget)
    => _mapper.Map<FundDto>(budget.RenameFund(command.FundId, command.Name));
}
