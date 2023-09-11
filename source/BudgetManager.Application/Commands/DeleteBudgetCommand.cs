namespace BudgetManager.Application.Commands;

using System.Text.Json.Serialization;

public record DeleteBudgetCommand([property: JsonIgnore()] string UserId) : IRequest<Unit>;

public class DeleteBudgetCommandHandler : IRequestHandler<DeleteBudgetCommand, Unit>
{
  private IUserBudgetRepository _repository;
  public DeleteBudgetCommandHandler(IUserBudgetRepository repository) => _repository = repository;

  public async Task<Unit> Handle(DeleteBudgetCommand command, CancellationToken cancellationToken)
    => await _repository.Delete(command.UserId).ContinueWith(_ => Unit.Value);
}

public class DeleteBudgetCommandValidator : AbstractValidator<DeleteBudgetCommand>
{
  public DeleteBudgetCommandValidator(IUserBudgetRepository repository)
  {
    RuleFor(x => x.UserId)
      .NotEmpty();

    RuleFor(x => x.UserId)
      .MustAsync(async (id, cancellation) => await repository.Exists(id))
      .WithMessage("Budget does not exist");
  }
}