namespace BudgetManager.Application.Commands;

using System.Text.Json.Serialization;

public record CreateBudgetCommand([property: JsonIgnore()] string UserId) : IRequest<bool>;

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

public class CreateBudgetCommandValidator : AbstractValidator<CreateBudgetCommand>
{
  public CreateBudgetCommandValidator(IUserBudgetRepository repository, AppConfig config)
  {
    RuleFor(x => x.UserId)
      .NotEmpty();
  }
}