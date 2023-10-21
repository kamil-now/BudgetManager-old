namespace BudgetManager.Application.Features.BudgetManagement;

using AutoMapper;
using BudgetManager.Domain.Models;
using BudgetManager.Infrastructure;

public class CreateAllocationCommandHandler
  : BudgetCommandHandler<CreateAllocationCommand, string>
{
  public CreateAllocationCommandHandler(IUserBudgetRepository repo, IMapper map)
  : base(repo, map)
  {
  }

  public override string ModifyBudget(CreateAllocationCommand command, Budget budget)
  {
    var id = Guid.NewGuid().ToString();

    budget.AddOperation(
      new Allocation(
        id,
        command.Title,
        command.Value,
        command.TargetFundId,
        command.Date,
        command.Description ?? string.Empty,
        DateTime.Now
        )
      );

    return id;
  }
}
