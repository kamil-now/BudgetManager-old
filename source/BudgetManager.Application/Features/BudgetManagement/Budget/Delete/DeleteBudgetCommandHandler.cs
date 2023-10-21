namespace BudgetManager.Application.Features.BudgetManagement;

public class DeleteBudgetCommandHandler : IRequestHandler<DeleteBudgetCommand, Unit>
{
  private IUserBudgetRepository _repository;
  public DeleteBudgetCommandHandler(IUserBudgetRepository repository) => _repository = repository;

  public async Task<Unit> Handle(DeleteBudgetCommand command, CancellationToken cancellationToken)
    => await _repository.Delete(command.UserId).ContinueWith(_ => Unit.Value);
}
