namespace BudgetManager.Application.Features.BudgetManagement;

using AutoMapper;
using BudgetManager.Domain.Models;
using BudgetManager.Infrastructure;

public class UpdateExpenseCommandHandler(IUserBudgetRepository repo, IMapper map)
  : UpdateMoneyOperationCommandHandler<UpdateExpenseCommand, Expense, ExpenseDto>(repo, map)
{
  protected override void Update(Expense operation, UpdateExpenseCommand command)
  => operation.Update(
      command.FundId,
      command.AccountId,
      command.Title,
      command.Value,
      command.Date,
      command.Description
    );

  protected override ExpenseDto CompleteDto(ExpenseDto dto, Budget budget)
    => dto with
    {
      Type = MoneyOperationType.Expense,
      AccountName = budget.Accounts.First(x => x.Id == dto.AccountId).Name,
      FundName = budget.Funds.First(x => x.Id == dto.FundId).Name,
    };
}
