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
  private IMapper _mapper;
  public CreateAllocationCommandValidator(IUserBudgetRepository repository, AppConfig config, IMapper mapper) : base(repository)
  {
    _mapper = mapper;

    RuleFor(x => x.Title)
      .NotEmpty()
      .MaximumLength(config.MaxTitleLength);

    RuleFor(x => x.Description)
      .MaximumLength(config.MaxContentLength);

    RuleFor(x => x.Value.Amount)
      .GreaterThan(0);
  }

  protected override void RulesWhenBudgetExists()
  {
    RuleFor(x => x)
      .Must((command, cancellation)
        => !string.IsNullOrEmpty(command.Category) ^ !string.IsNullOrEmpty(command.FundId)
      ).WithMessage("Either Fund id or Spending Fund category must be defined.").DependentRules(() =>
      {
        RuleFor(x => x)
          .MustAsync(async (command, cancellation) =>
          {
            var budget = await repository.Get(command.UserId);
            return budget!.Funds?.Any(x => x.Id == command.FundId) ?? false;
          }).When(command => command.Category is null && command.FundId is not null)
          .WithMessage("Fund does not exist.");

        RuleFor(x => x)
          .MustAsync(async (command, cancellation) =>
          {
            var budget = await repository.Get(command.UserId);
            return budget!.SpendingFund?.Categories?.Keys.Any(x => x == command.Category) ?? false;
          }).When(command => command.Category is not null && command.FundId is null)
          .WithMessage(command => $"Category '{command.Category}' does not exist.");
      });

    RuleFor(x => x)
      .MustAsync(async (command, cancellation) =>
      {
        var budgetEntity = await repository.Get(command.UserId);
        var budget = _mapper.Map<Budget>(budgetEntity);

        var availableFunds = budget.SpendingFund.Balance.ContainsKey(command.Value.Currency) ? budget.SpendingFund.Balance[command.Value.Currency] : 0;
        return availableFunds >= command.Value.Amount;
      }).WithMessage($"Insufficient funds.");
  }
}