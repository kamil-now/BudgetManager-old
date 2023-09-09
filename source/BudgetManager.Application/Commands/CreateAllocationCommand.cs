namespace BudgetManager.Application.Commands;

using System.Text.Json.Serialization;
using AutoMapper;
using BudgetManager.Domain.Models;
using BudgetManager.Infrastructure;

public record CreateAllocationCommand(
  [property: JsonIgnore()] string UserId,
  string Title,
  Money Value,
  string? Date,
  string TargetFundId,
  string? Description
  ) : IRequest<string>, IBudgetCommand;

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
    var now = DateOnly.FromDateTime(DateTime.Now);
    var date = command.Date is null ? now : DateOnly.FromDateTime(DateTime.Parse(command.Date));

    budget.AddOperation(
      new Allocation(
        id,
        command.Title,
        command.Value,
        command.TargetFundId,
        date,
        command.Description ?? string.Empty,
        DateTime.Now
        )
      );

    return id;
  }
}

public class CreateAllocationCommandValidator
  : BudgetCommandValidator<CreateAllocationCommand>
{
  public CreateAllocationCommandValidator(IUserBudgetRepository repository, AppConfig config) : base(repository)
  {
    RuleFor(x => x.Title)
      .MaximumLength(config.MaxTitleLength);

    RuleFor(x => x.Description)
      .MaximumLength(config.MaxContentLength);

    RuleFor(x => x.Value.Amount)
      .GreaterThan(0);
  }

  protected override void RulesWhenBudgetExists()
  {
    RuleFor(x => x)
      .MustAsync(async (command, cancellation) => {
        var unallocated = (await repository.Get(command.UserId)).Unallocated;
        return (unallocated?.ContainsKey(command.Value.Currency) ?? false) && unallocated[command.Value.Currency] >= command.Value.Amount;
      }).WithMessage("Insufficient unallocated funds.");

    RuleFor(x => x)
      .MustAsync(async (command, cancellation)
        => (await repository.Get(command.UserId)).Funds?.Any(x => x.Id == command.TargetFundId && !x.IsDeleted) ?? false)
      .WithMessage("Target fund is deleted or does not exist.");
  }
}