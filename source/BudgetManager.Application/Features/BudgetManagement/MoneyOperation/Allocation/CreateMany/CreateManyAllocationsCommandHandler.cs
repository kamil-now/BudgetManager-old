namespace BudgetManager.Application.Features.BudgetManagement;

using AutoMapper;
using BudgetManager.Domain.Models;
using BudgetManager.Infrastructure;

public class CreateManyAllocationsCommandHandler(IUserBudgetRepository repo, IMapper map)
  : BudgetCommandHandler<CreateManyAllocationsCommand, Unit>(repo, map)
{
  public override Unit ModifyBudget(CreateManyAllocationsCommand command, Budget budget)
  {
    foreach (var allocation in command.Allocations)
    {
      var id = Guid.NewGuid().ToString();

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

    return Unit.Value;
  }
}
