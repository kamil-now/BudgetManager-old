namespace BudgetManager.Application.Commands;

using System.Text.Json.Serialization;
using AutoMapper;
using BudgetManager.Domain.Models;
using BudgetManager.Infrastructure;

public record CreateFundTransferCommand(
  [property: JsonIgnore()] string UserId,
  string Title,
  Money Value,
  string? Date,
  string? Description,
  string SourceFundId,
  string TargetFundId
  ) : IRequest<string>, IBudgetCommand;

public class CreateFundTransferCommandHandler
  : BudgetCommandHandler<CreateFundTransferCommand, string>
{
  public CreateFundTransferCommandHandler(IUserBudgetRepository repo, IMapper map)
  : base(repo, map)
  {
  }

  public override string ModifyBudget(CreateFundTransferCommand command, Budget budget)
  {
    var id = Guid.NewGuid().ToString();
    var now = DateOnly.FromDateTime(DateTime.Now);
    var date = command.Date is null ? now : DateOnly.FromDateTime(DateTime.Parse(command.Date));

    budget.AddOperation(
      new FundTransfer(
        id,
        command.Title,
        command.Value,
        command.SourceFundId,
        command.TargetFundId,
        date,
        command.Description ?? string.Empty,
        DateTime.Now
        )
      );

    return id;
  }
}

public class CreateFundTransferCommandValidator
  : BudgetCommandValidator<CreateFundTransferCommand>
{
  public CreateFundTransferCommandValidator(IUserBudgetRepository repository, AppConfig appConfig) : base(repository)
  {
    RuleFor(x => x.Title)
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
      .Must((command, cancellation) => !string.IsNullOrEmpty(command.SourceFundId))
        .WithMessage("Source id must be defined.")
      .Must((command, cancellation) => !string.IsNullOrEmpty(command.TargetFundId))
        .WithMessage("Target id must be defined.")
      .DependentRules(() =>
      {
        RuleFor(x => x)
          .MustAsync(async (command, cancellation) =>
          {
            var budget = await repository.Get(command.UserId);
            return budget!.Funds?.Any(x => x.Id == command.SourceFundId) ?? false;
          })
          .WithMessage(command => $"Source Fund with id '{command.SourceFundId}' does not exist in the budget.")
          .DependentRules(() =>
          {
            RuleFor(x => x)
              .MustAsync(async (command, cancellation) =>
              {
                var budget = await repository.Get(command.UserId);
                var fund = budget!.Funds!.First(x => x.Id == command.SourceFundId);
                return fund!.Balance!.Keys.Contains(command.Value.Currency) && fund!.Balance![command.Value.Currency] >= command.Value.Amount;
              })
              .WithMessage("Insufficient funds.");
          });

        RuleFor(x => x)
          .MustAsync(async (command, cancellation) =>
          {
            var budget = await repository.Get(command.UserId);
            return budget!.Funds?.Any(x => x.Id == command.TargetFundId) ?? false;
          }).WithMessage(command => $"Target fund with id '{command.TargetFundId}' does not exist in the budget.");
      });
  }
}