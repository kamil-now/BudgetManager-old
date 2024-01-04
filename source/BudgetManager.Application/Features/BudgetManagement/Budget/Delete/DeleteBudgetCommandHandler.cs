namespace BudgetManager.Application.Features.BudgetManagement;

public class DeleteBudgetCommandHandler(IUserBudgetRepository _repository) : IRequestHandler<DeleteBudgetCommand, Unit>
{
  public async Task<Unit> Handle(DeleteBudgetCommand command, CancellationToken cancellationToken)
  => await _repository.Delete(command.UserId).ContinueWith(_ => Unit.Value);
}
