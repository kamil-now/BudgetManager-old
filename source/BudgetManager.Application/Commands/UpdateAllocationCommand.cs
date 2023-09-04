namespace BudgetManager.Application.Commands;

using AutoMapper;
using BudgetManager.Domain.Models;
using BudgetManager.Infrastructure;

public record UpdateAllocationCommand(
  string UserId,
  string OperationId,
  string? Title,
  Money? Value,
  string? Date,
  string? TargetFundId,
  string? Description
  ) : UpdateOperationCommand<Allocation, AllocationDto>(UserId, OperationId);

public class UpdateAllocationCommandHandler : UpdateOperationCommandHandler<UpdateAllocationCommand, Allocation, AllocationDto>
{
  public UpdateAllocationCommandHandler(IUserBudgetRepository repo, IMapper map)
  : base(repo, map)
  {
  }

  protected override void Update(Allocation operation, UpdateAllocationCommand command)
    => operation.Update(
        command.TargetFundId,
        command.Title,
        command.Value,
        command.Date,
        command.Description
      );
}

public class UpdateAllocationCommandValidator : UpdateOperationCommandValidator<UpdateAllocationCommand>
{
  public UpdateAllocationCommandValidator(IUserBudgetRepository repository, AppConfig config) : base(repository)
  {
    RuleFor(x => x.Title)
      .Must(title => title is null || title.Length <= config.MaxTitleLength);

    RuleFor(x => x.Value).ISO_4217_Currency(allowNull: true);

    RuleFor(x => x)
      .MustAsync(async (command, cancellation) =>
      {
        if (command.TargetFundId is null)
          return true;
        var budget = await repository.Get(command.UserId);
        return budget.Funds?.Any(x => x.Id == command.TargetFundId) ?? false;
      }).WithMessage(command => $"Fund with id {command.TargetFundId} does not exist in the budget");
  }
}