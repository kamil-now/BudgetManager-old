namespace BudgetManager.Application.Commands;

using System.Text.Json.Serialization;
using AutoMapper;
using BudgetManager.Domain.Models;
using BudgetManager.Infrastructure;

public record CreateAccountCommand(
  [property: JsonIgnore()] string UserId, string Name, decimal InitialAmount, string Currency)
  : IRequest<string>, IBudgetCommand;

public class CreateAccountCommandHandler : BudgetCommandHandler<CreateAccountCommand, string>
{
  public CreateAccountCommandHandler(IUserBudgetRepository repo, IMapper map)
  : base(repo, map)
  {
  }

  public override string ModifyBudget(CreateAccountCommand command, Budget budget)
    => budget.AddAccount(command.Name, new Money(command.InitialAmount, command.Currency));
}

public class CreateAccountCommandValidator : BudgetCommandValidator<CreateAccountCommand>
{
  public CreateAccountCommandValidator(IUserBudgetRepository repository, AppConfig config) : base(repository)
  {
    RuleFor(x => x.Currency)
      .ISO_4217_Currency();

    RuleFor(x => x.InitialAmount)
      .GreaterThanOrEqualTo(0);

    RuleFor(x => x.Name)
      .NotEmpty()
      .MaximumLength(config.MaxTitleLength);
  }
}