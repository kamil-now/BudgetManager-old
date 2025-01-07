namespace BudgetManager.Application.Features.BudgetManagement;

using AutoMapper;
using BudgetManager.Domain.Models;
using BudgetManager.Infrastructure;

public class CreateManyAllocationsCommandHandler(IUserBudgetRepository repo, IMapper map)
  : BudgetCommandHandler<CreateManyAllocationsCommand, string>(repo, map)
{
  public override string ModifyBudget(CreateManyAllocationsCommand command, Budget budget)
  {
    var id = Guid.NewGuid().ToString();
    foreach (var allocation in command.Allocations)
    {

      budget.AddOperation(
        new Allocation(
          id,
          allocation.Title,
          allocation.Value,
          allocation.TargetFundId,
          allocation.Date,
          allocation.Description ?? string.Empty,
          DateTime.Now
          )
        );
    }

    return id;
  }
}
