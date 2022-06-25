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
  {
    var id = Guid.NewGuid().ToString();

    budget.AddAccount(new Account(id, command.Name, new Money(command.InitialAmount, command.Currency)));

    return id;
  }
}

public class CreateAccountCommandValidator : BudgetCommandValidator<CreateAccountCommand>
{
  public CreateAccountCommandValidator(IUserBudgetRepository repository) : base(repository)
  {
    RuleFor(x => x.Currency)
      .NotEmpty()
      .MaximumLength(3);

    RuleFor(x => x.Name)
      .NotEmpty()
      .MaximumLength(50);
  }
}