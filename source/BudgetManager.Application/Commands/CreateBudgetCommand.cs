namespace BudgetManager.Application.Commands;

using System.Text.Json.Serialization;

public record CreateBudgetCommand([property: JsonIgnore()] string UserId, string DefaultFundName) : IRequest<Unit>;

public class CreateBudgetCommandHandler : IRequestHandler<CreateBudgetCommand, Unit>
{
  private IUserBudgetRepository _repository;
  public CreateBudgetCommandHandler(IUserBudgetRepository repository) => _repository = repository;

  public async Task<Unit> Handle(CreateBudgetCommand command, CancellationToken cancellationToken)
    => await _repository.Create(command.UserId, command.DefaultFundName).ContinueWith(_ => Unit.Value);
}

public class CreateBudgetCommandValidator : AbstractValidator<CreateBudgetCommand>
{
  public CreateBudgetCommandValidator(IUserBudgetRepository repository, AppConfig config)
  {
    RuleFor(x => x.UserId)
      .NotEmpty();

    RuleFor(x => x.UserId)
      .MustAsync(async (id, cancellation) => !await repository.Exists(id))
        .WithMessage("Budget already exists")
        .DependentRules(() =>
        {
          RuleFor(x => x.DefaultFundName)
          .NotEmpty()
          .MaximumLength(config.MaxTitleLength);
        });
  }
}