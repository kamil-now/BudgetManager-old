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
  string? TargetFundId
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
    var date = command.Date is null ? now : DateOnly.Parse(command.Date);

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
      .NotEmpty()
      .MaximumLength(appConfig.MaxTitleLength);

    RuleFor(x => x.Description)
      .MaximumLength(appConfig.MaxContentLength);

    RuleFor(x => x.Value.Amount)
      .GreaterThan(0);
  }

  protected override void RulesWhenBudgetExists()
  {
    RuleFor(x => x)
      .Must((command, cancellation) =>
        !string.IsNullOrEmpty(command.SourceFundId) && !string.IsNullOrEmpty(command.TargetFundId)
      ).WithMessage("Source fund id and target fund id must be defined.")
      .DependentRules(() =>
      {
        RuleFor(x => x)
          .MustAsync(async (command, cancellation) =>
          {
            var budget = await repository.Get(command.UserId);
            return budget!.Funds?.Any(x => x.Id == command.SourceFundId) ?? false;
          }).WithMessage("Source fund with a given id does not exist in the budget");

        RuleFor(x => x)
          .MustAsync(async (command, cancellation) =>
          {
            if (command.TargetFundId is null)
              return true;
            var budget = await repository.Get(command.UserId);
            return budget!.Funds?.Any(x => x.Id == command.TargetFundId) ?? false;
          }).WithMessage("Target fund with a given id does not exist in the budget");

        // TODO sufficient funds
      });
  }
}