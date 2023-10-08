namespace BudgetManager.Application.Commands;

using System.Text.Json.Serialization;
using AutoMapper;
using BudgetManager.Domain.Models;
using BudgetManager.Infrastructure;

public record RecalculateBudgetCommand(
  [property: JsonIgnore()] string UserId
) : IRequest<string>, IBudgetCommand;

public class RecalculateBudgetCommandHandler
  : BudgetCommandHandler<RecalculateBudgetCommand, string>
{
  public RecalculateBudgetCommandHandler(IUserBudgetRepository repo, IMapper map)
  : base(repo, map)
  {
  }

  public override string ModifyBudget(RecalculateBudgetCommand command, Budget budget)
  {
    budget.Recalculate();
    return "OK";
  }
}

public class RecalculateBudgetCommandValidator
  : BudgetCommandValidator<RecalculateBudgetCommand>
{
  public RecalculateBudgetCommandValidator(IUserBudgetRepository repository, AppConfig config) : base(repository)
  {
  }
}