public record CreateBudgetCommand(string UserId) : IRequest<string>;

public class CreateBudgetCommandHandler : IRequestHandler<CreateBudgetCommand, string>
{
  private IUserBudgetRepository _repository;
  public CreateBudgetCommandHandler(IUserBudgetRepository repository) => _repository = repository;

  public async Task<string> Handle(CreateBudgetCommand command, CancellationToken cancellationToken)
    => await _repository.Create(command.UserId);
}

public class CreateBudgetCommandValidator : AbstractValidator<CreateBudgetCommand>
{
  public CreateBudgetCommandValidator(IUserBudgetRepository repository)
  {
    RuleFor(x => x.UserId)
      .MustAsync(async (id, cancellation) => !await repository.Exists(id))
        .WithMessage("Budget already exists");
  }
}