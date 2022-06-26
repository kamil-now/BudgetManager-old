namespace BudgetManager.Application.Commands;

using System.Text.Json.Serialization;
using AutoMapper;
using BudgetManager.Domain.Models;
using BudgetManager.Infrastructure;

public record ToggleExpenseCommand([property: JsonIgnore()] string UserId, string OperationId)
  : IRequest<Unit>, IOperationCommand;

public class ToggleExpenseCommandHandler : BudgetCommandHandler<ToggleExpenseCommand, Unit>
{
  public ToggleExpenseCommandHandler(IUserBudgetRepository repo, IMapper map)
  : base(repo, map)
  {
  }

  public override Unit ModifyBudget(ToggleExpenseCommand command, Budget budget)
  {
    budget.ToggleExpenseIsConfirmed(command.OperationId);
    return Unit.Value;
  }
}

public class ConfirmExpenseCommandValidator : ExpenseCommandValidator<ToggleExpenseCommand>
{
  public ConfirmExpenseCommandValidator(IUserBudgetRepository repository) : base(repository)
  {
  }
}