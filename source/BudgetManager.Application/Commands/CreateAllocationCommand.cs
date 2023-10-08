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
    var date = DateTime.Now;
    if (command.Date is not null)
    {
      if (DateTime.TryParse(command.Date, out var commandDate))
      {
        date = commandDate;
      }
      else
      {
        throw new Exception("Invalid date.");
      }
    }

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
      .NotEqual(0);
  }

  protected override void RulesWhenBudgetExists()
  {
    RuleFor(x => x)
      .MustAsync(async (command, cancellation)
        => (await repository.Get(command.UserId)).Funds?.Any(x => x.Id == command.TargetFundId && !x.IsDeleted) ?? false)
      .WithMessage("Target fund is deleted or does not exist.");
  }
}