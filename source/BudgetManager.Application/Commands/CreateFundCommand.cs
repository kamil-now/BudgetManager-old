namespace BudgetManager.Application.Commands;

using AutoMapper;
using BudgetManager.Domain.Models;
using BudgetManager.Infrastructure;

public record CreateFundCommand(string UserId, string Name, Dictionary<string, decimal> InitialBalance)
  : IRequest<string>, IBudgetCommand;

public class CreateFundCommandHandler : BudgetCommandHandler<CreateFundCommand, string>
{
  public CreateFundCommandHandler(IUserBudgetRepository repo, IMapper map)
  : base(repo, map)
  {
  }

  public override string ModifyBudget(CreateFundCommand command, Budget budget)
  {
    var id = Guid.NewGuid().ToString();

    budget.AddFund(new Fund(id, command.Name, new Balance(command.InitialBalance)));

    return id;
  }
}

public class CreateFundCommandValidator : BudgetCommandValidator<CreateFundCommand>
{
  public CreateFundCommandValidator(IUserBudgetRepository repository) : base(repository)
  {
  }
}