namespace BudgetManager.Application.Features.BudgetManagement;

public class CreateBudgetCommandHandler : IRequestHandler<CreateBudgetCommand, bool>
{
  private IUserBudgetRepository _repository;
  public CreateBudgetCommandHandler(IUserBudgetRepository repository) => _repository = repository;

  public async Task<bool> Handle(CreateBudgetCommand command, CancellationToken cancellationToken)
  {
    var budgetExists = await _repository.Exists(command.UserId);
    if (!budgetExists)
    {
      await _repository.Create(command.UserId);
      return true;
    }
    return false;
  }
}
