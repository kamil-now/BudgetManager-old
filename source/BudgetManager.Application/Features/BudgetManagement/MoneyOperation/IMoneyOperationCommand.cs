namespace BudgetManager.Application.Features.BudgetManagement;

public interface IMoneyOperationCommand : IBudgetCommand
{
  string Id { get; init; }
}
