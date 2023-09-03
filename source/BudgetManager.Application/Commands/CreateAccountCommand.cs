namespace BudgetManager.Application.Commands;

using System.Text.Json.Serialization;
using AutoMapper;
using BudgetManager.Domain.Models;
using BudgetManager.Infrastructure;

public record CreateAccountCommand(
  [property: JsonIgnore()] string UserId, string Name, decimal InitialAmount, string Currency, string? FundId = null)
  : IRequest<string>, IBudgetCommand;

public class CreateAccountCommandHandler : BudgetCommandHandler<CreateAccountCommand, string>
{
  public CreateAccountCommandHandler(IUserBudgetRepository repo, IMapper map)
  : base(repo, map)
  {
  }

  public override string ModifyBudget(CreateAccountCommand command, Budget budget)
    => budget.AddAccount(command.Name, command.FundId, new Money(command.InitialAmount, command.Currency));
}

public class CreateAccountCommandValidator : BudgetCommandValidator<CreateAccountCommand>
{
  public CreateAccountCommandValidator(IUserBudgetRepository repository, AppConfig config) : base(repository)
  {
    RuleFor(x => x.Currency)
      .NotEmpty()
      .ISO_4217_Currency();

    RuleFor(x => x.InitialAmount)
      .GreaterThanOrEqualTo(0);

    RuleFor(x => x.Name)
      .NotEmpty()
      .MaximumLength(config.MaxTitleLength);
  }

  protected override void RulesWhenBudgetExists()
  {
    RuleFor(x => x)
    .Must(command => command.InitialAmount <= 0 || !string.IsNullOrEmpty(command.FundId))
    .WithMessage("When initial amount is greater than 0, fund id must be defined.")
    .DependentRules(() =>
    {
      RuleFor(x => x)
      .MustAsync(async (command, cancellation)
        => string.IsNullOrEmpty(command.FundId) || ((await repository.Get(command.UserId)).Funds?.Any(x => x.Id == command.FundId && !x.IsDeleted) ?? false))
      .WithMessage("Fund is deleted or does not exist.");
    });
  }
}