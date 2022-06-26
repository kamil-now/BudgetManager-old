namespace BudgetManager.Application.Commands;

using System.Text.Json.Serialization;
using AutoMapper;
using BudgetManager.Domain.Models;
using BudgetManager.Infrastructure;

public record UpdateAllocationCommand(
  [property: JsonIgnore()]
  string UserId,
  string OperationId,
  string? Title,
  Money? Value,
  string? Date,
  string? FundId,
  string? Description
  ) : IRequest<Unit>, IOperationCommand;

public class UpdateAllocationCommandHandler
  : BudgetCommandHandler<UpdateAllocationCommand, Unit>
{
  public UpdateAllocationCommandHandler(IUserBudgetRepository repo, IMapper map)
  : base(repo, map)
  {
  }

  public override Unit ModifyBudget(UpdateAllocationCommand command, Budget budget)
  {
    var allocation = budget.Operations.First(x => x.Id == command.OperationId) as Allocation;

    allocation!.Update(command.FundId, command.Title, command.Value, command.Date, command.Description);

    return Unit.Value;
  }
}


public class AllocationCommandValidator<T> : BudgetCommandValidator<T> where T : IRequest<Unit>, IOperationCommand
{
  public AllocationCommandValidator(IUserBudgetRepository repository) : base(repository)
  {
    RuleFor(x => x)
      .MustAsync(async (command, cancellation) =>
      {
        var budget = await repository.Get(command.UserId);
        return budget!.Allocations?.Any(x => x.Id == command.OperationId) ?? false;
      }).WithMessage("Allocation with a given id does not exist in the budget");
  }
}

public class UpdateAllocationCommandValidator : AllocationCommandValidator<UpdateAllocationCommand>
{
  public UpdateAllocationCommandValidator(IUserBudgetRepository repository) : base(repository)
  {
    RuleFor(x => x.Title)
      .Must(title => title is null || title.Length <= 50);

    RuleFor(x => x.Value)
      .Must(value => value is null || value?.Currency.Length == 3);

    RuleFor(x => x)
      .MustAsync(async (command, cancellation) =>
      {
        if (command.FundId is null)
          return true;
        var budget = await repository.Get(command.UserId);
        return budget!.Funds?.Any(x => x.Id == command.FundId) ?? false;
      }).WithMessage("Fund with a given id does not exist in the budget");
  }
}