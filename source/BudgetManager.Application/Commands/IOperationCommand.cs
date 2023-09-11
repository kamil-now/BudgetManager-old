namespace BudgetManager.Application.Commands;

public interface IOperationCommand : IBudgetCommand
{
  string Id { get; init; }
}
