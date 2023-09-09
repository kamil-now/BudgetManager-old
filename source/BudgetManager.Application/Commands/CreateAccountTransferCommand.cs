namespace BudgetManager.Application.Commands;

using System.Text.Json.Serialization;
using AutoMapper;
using BudgetManager.Domain.Models;
using BudgetManager.Infrastructure;

public record CreateAccountTransferCommand(
  [property: JsonIgnore()] string UserId,
  string Title,
  Money Value,
  string? Date,
  string? Description,
  string SourceAccountId,
  string TargetAccountId
  ) : IRequest<string>, IBudgetCommand;

public class CreateAccountTransferCommandHandler
  : BudgetCommandHandler<CreateAccountTransferCommand, string>
{
  public CreateAccountTransferCommandHandler(IUserBudgetRepository repo, IMapper map)
  : base(repo, map)
  {
  }

  public override string ModifyBudget(CreateAccountTransferCommand command, Budget budget)
  {
    var id = Guid.NewGuid().ToString();
    var now = DateOnly.FromDateTime(DateTime.Now);
    var date = command.Date is null ? now : DateOnly.Parse(command.Date);

    budget.AddOperation(
      new AccountTransfer(
        id,
        command.Title,
        command.Value,
        command.SourceAccountId,
        command.TargetAccountId,
        date,
        command.Description ?? string.Empty,
        DateTime.Now
        )
      );

    return id;
  }
}

public class CreateAccountTransferCommandValidator
  : BudgetCommandValidator<CreateAccountTransferCommand>
{
  public CreateAccountTransferCommandValidator(IUserBudgetRepository repository, AppConfig appConfig) : base(repository)
  {
    RuleFor(x => x.Title)
      .NotEmpty()
      .MaximumLength(appConfig.MaxTitleLength);

    RuleFor(x => x.Description)
      .MaximumLength(appConfig.MaxContentLength);

    RuleFor(x => x.Value.Amount)
      .GreaterThan(0);

    RuleFor(x => x.Value.Currency)
      .ISO_4217_Currency();
  }

  protected override void RulesWhenBudgetExists()
  {
    RuleFor(x => x)
      .Must((command, cancellation) => !string.IsNullOrEmpty(command.SourceAccountId))
        .WithMessage("Source id must be defined.")
      .Must((command, cancellation) => !string.IsNullOrEmpty(command.TargetAccountId))
        .WithMessage("Target id must be defined.")
      .DependentRules(() =>
      {
        RuleFor(x => x)
          .MustAsync(async (command, cancellation) =>
          {
            var budget = await repository.Get(command.UserId);
            return budget!.Accounts?.Any(x => x.Id == command.SourceAccountId) ?? false;
          })
          .WithMessage(command => $"Source Account with id '{command.SourceAccountId}' does not exist in the budget.")
          .DependentRules(() =>
          {
            RuleFor(x => x)
              .MustAsync(async (command, cancellation) =>
              {
                var budget = await repository.Get(command.UserId);
                var account = budget!.Accounts!.First(x => x.Id == command.SourceAccountId);
                return account!.Balance!.ContainsKey(command.Value.Currency) && account!.Balance?[command.Value.Currency] >= command.Value.Amount;
              })
              .WithMessage("Insufficient funds.");
          });

        RuleFor(x => x)
          .MustAsync(async (command, cancellation) =>
          {
            var budget = await repository.Get(command.UserId);
            return budget!.Accounts?.Any(x => x.Id == command.TargetAccountId) ?? false;
          }).WithMessage(command => $"Target account with id '{command.TargetAccountId}' does not exist in the budget.");
      });
  }
}