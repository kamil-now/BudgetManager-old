namespace BudgetManager.Application.Features.BudgetManagement;

using AutoMapper;
using BudgetManager.Domain.Models;
using BudgetManager.Infrastructure;

public class CreateExpenseCommandHandler
  : BudgetCommandHandler<CreateExpenseCommand, string>
{
  public CreateExpenseCommandHandler(IUserBudgetRepository repo, IMapper map)
  : base(repo, map)
  {
  }

  public override string ModifyBudget(CreateExpenseCommand command, Budget budget)
  {
    var id = Guid.NewGuid().ToString();

    budget.AddOperation(
      new Expense(
        id,
        command.Title,
        command.Value,
        command.Date,
        command.AccountId,
        command.FundId,
        command.Description ?? string.Empty,
        DateTime.Now
        )
      );

    return id;
  }
}
