namespace BudgetManager.Application.Features.BudgetManagement;

public class CreateBudgetCommandHandler : IRequestHandler<CreateBudgetCommand, bool>
{
  private IUserBudgetRepository _repository;
  private AppConfig _appConfig;

  public CreateBudgetCommandHandler(IUserBudgetRepository repository, AppConfig appConfig)
  {
    _repository = repository;
    _appConfig = appConfig;
  }

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
