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
  string? Description,
  string? FundId,
  string? Category
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
    var date = command.Date is null ? now : DateOnly.Parse(command.Date);

    budget.AddOperation(
      new Allocation(
        id,
        command.Title,
        command.Value,
        date,
        command.Description ?? string.Empty,
        DateTime.Now,
        command.FundId,
        command.Category
        )
      );

    return id;
  }
}

public class CreateAllocationCommandValidator
  : BudgetCommandValidator<CreateAllocationCommand>
{
  public CreateAllocationCommandValidator(IUserBudgetRepository repository) : base(repository)
  {
    RuleFor(x => x.Title)
      .NotEmpty()
      .MaximumLength(50);

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