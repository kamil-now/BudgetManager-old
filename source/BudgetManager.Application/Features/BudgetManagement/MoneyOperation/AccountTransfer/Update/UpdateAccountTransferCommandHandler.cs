namespace BudgetManager.Application.Features.BudgetManagement;

using AutoMapper;
using BudgetManager.Domain.Models;
using BudgetManager.Infrastructure;

public class UpdateAccountTransferCommandHandler : UpdateMoneyOperationCommandHandler<UpdateAccountTransferCommand, AccountTransfer, AccountTransferDto>
{
  public UpdateAccountTransferCommandHandler(IUserBudgetRepository repo, IMapper map)
  : base(repo, map)
  {
  }
  protected override void Update(AccountTransfer operation, UpdateAccountTransferCommand command)
    => operation.Update(
        command.AccountId,
        command.TargetAccountId,
        command.Title,
        command.Value,
        command.Date,
        command.Description
      );
  protected override AccountTransferDto CompleteDto(AccountTransferDto dto, Budget budget)
    => dto with
    {
      Type = MoneyOperationType.AccountTransfer,
      AccountName = budget.Accounts.First(x => x.Id == dto.AccountId).Name,
      TargetAccountName = budget.Accounts.First(x => x.Id == dto.TargetAccountId).Name,
    };
}
