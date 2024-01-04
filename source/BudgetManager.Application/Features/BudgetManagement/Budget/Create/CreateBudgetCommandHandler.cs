namespace BudgetManager.Application.Features.BudgetManagement;

public class CreateBudgetCommandHandler(IUserBudgetRepository _repository, AppConfig _appConfig) : IRequestHandler<CreateBudgetCommand, bool>
{
  public async Task<bool> Handle(CreateBudgetCommand command, CancellationToken cancellationToken)
  {
    var budgetExists = await _repository.Exists(command.UserId);
    if (!budgetExists)
    {
      if (_appConfig.CreateBudgetsWithExampleData)
      {
        await _repository.CreateWithSampleData(command.UserId);
      }
      else
      {
        await _repository.Create(command.UserId);
      }
      return true;
    }
    return false;
  }
}
