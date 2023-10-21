namespace BudgetManager.Application.Features.BudgetManagement;

using BudgetManager.Infrastructure;
using ISO._4217;

public class CreateAccountCommandValidator : BudgetCommandValidator<CreateAccountCommand>
{
  public CreateAccountCommandValidator(IUserBudgetRepository repository, AppConfig config) : base(repository)
  {
    RuleFor(x => x.Name)
      .NotEmpty()
      .MaximumLength(config.MaxTitleLength);

    RuleFor(x => x)
      .Must((command, cancellation) 
      => command.InitialBalance.Keys.All(currency => CurrencyCodesResolver.Codes.Any(c => c.Code == currency)))
      .WithMessage("Account currencies must comply with ISO 4217.");

      
    RuleFor(x => x)
      .Must((command, cancellation) 
      => command.InitialBalance.Values.All(value => value >= 0))
      .WithMessage("Initial balance values must be greater or equal 0.");
  }
}
