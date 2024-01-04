namespace BudgetManager.Application.Features.BudgetManagement;

using AutoMapper;
using BudgetManager.Domain.Models;
using BudgetManager.Infrastructure;

public class CreateIncomeCommandHandler(IUserBudgetRepository repo, IMapper map)
  : BudgetCommandHandler<CreateIncomeCommand, string>(repo, map)
{
  public override string ModifyBudget(CreateIncomeCommand command, Budget budget)
  {
    var id = Guid.NewGuid().ToString();

    budget.AddOperation(
      new Income(
        id,
        command.AccountId,
        command.Title,
        command.Value,
        command.Date,
        command.Description ?? string.Empty,
        DateTime.Now
        )
      );

    return id;
  }
}
