namespace BudgetManager.Application.Commands;

public interface IOperationCommand : IBudgetCommand
{
  string OperationId { get; init; }
}
