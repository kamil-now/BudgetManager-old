namespace BudgetManager.Application.Features.BudgetManagement;

using AutoMapper;
using BudgetManager.Domain.Models;
using BudgetManager.Infrastructure;

public class UpdateFundTransferCommandHandler(IUserBudgetRepository repo, IMapper map)
  : UpdateMoneyOperationCommandHandler<UpdateFundTransferCommand, FundTransfer, FundTransferDto>(repo, map)
{
  protected override void Update(FundTransfer operation, UpdateFundTransferCommand command)
  => operation.Update(
      command.FundId,
      command.TargetFundId,
      command.Title,
      command.Value,
      command.Date,
      command.Description
      );

  protected override FundTransferDto CompleteDto(FundTransferDto dto, Budget budget)
    => dto with
    {
      Type = MoneyOperationType.FundTransfer,
      FundName = budget.Funds.First(x => x.Id == dto.FundId).Name,
      TargetFundName = budget.Funds.First(x => x.Id == dto.TargetFundId).Name,
    };
}
