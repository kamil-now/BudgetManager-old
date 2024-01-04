namespace BudgetManager.Application.Features.BudgetManagement;

using AutoMapper;
using BudgetManager.Domain.Models;
using BudgetManager.Infrastructure;

public class UpdateIncomeCommandHandler(IUserBudgetRepository repo, IMapper map)
    : UpdateMoneyOperationCommandHandler<UpdateIncomeCommand, Income, IncomeDto>(repo, map)
{
  protected override void Update(Income operation, UpdateIncomeCommand command)
  => operation.Update(command.AccountId, command.Title, command.Value, command.Date, command.Description);

  protected override IncomeDto CompleteDto(IncomeDto dto, Budget budget)
    => dto with
    {
      Type = MoneyOperationType.Income,
      AccountName = budget.Accounts.First(x => x.Id == dto.AccountId).Name,
    };
}
