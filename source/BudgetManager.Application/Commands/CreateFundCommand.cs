namespace BudgetManager.Application.Commands;

using System.Text.Json.Serialization;
using AutoMapper;
using BudgetManager.Domain.Models;
using BudgetManager.Infrastructure;

public record CreateFundCommand([property: JsonIgnore()] string UserId, string Name)
  : IRequest<string>, IBudgetCommand;

public class CreateFundCommandHandler : BudgetCommandHandler<CreateFundCommand, string>
{
  public CreateFundCommandHandler(IUserBudgetRepository repo, IMapper map)
  : base(repo, map)
  {
  }

  public override string ModifyBudget(CreateFundCommand command, Budget budget) => budget.AddFund(command.Name);
}

public class CreateFundCommandValidator : BudgetCommandValidator<CreateFundCommand>
{
  public CreateFundCommandValidator(IUserBudgetRepository repository, AppConfig appConfig) : base(repository)
  {
    RuleFor(x => x.Name)
      .NotEmpty()
      .MaximumLength(appConfig.MaxTitleLength);
  }
}