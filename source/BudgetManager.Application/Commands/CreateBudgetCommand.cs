public record CreateBudgetCommand(string UserId) : IRequest<Unit>;

public class CreateBudgetCommandHandler : IRequestHandler<CreateBudgetCommand, Unit>
{
  private IUserBudgetRepository _repository;
  public CreateBudgetCommandHandler(IUserBudgetRepository repository) => _repository = repository;

  public async Task<Unit> Handle(CreateBudgetCommand command, CancellationToken cancellationToken)
    => await _repository.Create(command.UserId).ContinueWith(_ => Unit.Value);
}

public class CreateBudgetCommandValidator : AbstractValidator<CreateBudgetCommand>
{
  public CreateBudgetCommandValidator(IUserBudgetRepository repository)
  {
    RuleFor(x => x.UserId)
      .NotEmpty();

    RuleFor(x => x.UserId)
      .MustAsync(async (id, cancellation) => !await repository.Exists(id))
        .WithMessage("Budget already exists");
  }
}