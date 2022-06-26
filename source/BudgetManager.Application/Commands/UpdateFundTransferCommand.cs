namespace BudgetManager.Application.Commands;

using System.Text.Json.Serialization;
using AutoMapper;
using BudgetManager.Domain.Models;
using BudgetManager.Infrastructure;

public record UpdateFundTransferCommand(
  [property: JsonIgnore()]
  string UserId,
  string OperationId,
  string Title,
  Money Value,
  string? Date,
  string? Description,
  string? SourceFundId,
  string? TargetFundId,
  string? Category
  ) : IRequest<Unit>, IOperationCommand;

public class UpdateFundTransferCommandHandler
  : BudgetCommandHandler<UpdateFundTransferCommand, Unit>
{
  public UpdateFundTransferCommandHandler(IUserBudgetRepository repo, IMapper map)
  : base(repo, map)
  {
  }

  public override Unit ModifyBudget(UpdateFundTransferCommand command, Budget budget)
  {
    var fundTransfer = budget.Operations.First(x => x.Id == command.OperationId) as FundTransfer;

    fundTransfer!.Update(
      command.SourceFundId,
      command.TargetFundId,
      command.Category,
      command.Title,
      command.Value,
      command.Date,
      command.Description
    );

    return Unit.Value;
  }
}


public class FundTransferCommandValidator<T>
  : BudgetCommandValidator<T> where T : IRequest<Unit>, IOperationCommand
{
  public FundTransferCommandValidator(IUserBudgetRepository repository) : base(repository)
  {
    RuleFor(x => x)
      .MustAsync(async (command, cancellation) =>
      {
        var budget = await repository.Get(command.UserId);
        return budget!.FundTransfers?.Any(x => x.Id == command.OperationId) ?? false;
      }).WithMessage("FundTransfer with a given id does not exist in the budget");
  }
}

public class UpdateFundTransferCommandValidator
  : FundTransferCommandValidator<UpdateFundTransferCommand>
{
  public UpdateFundTransferCommandValidator(IUserBudgetRepository repository) : base(repository)
  {
    RuleFor(x => x.Title)
      .Must(title => title is null || title.Length <= 50);

    RuleFor(x => x.Category)
      .Must(category => category is null || category.Length <= 50);

    RuleFor(x => x)
      .Must(x => x.TargetFundId is null || x.Category is null);

    RuleFor(x => x)
      .MustAsync(async (command, cancellation) =>
      {
        if (command.SourceFundId is null)
          return true;
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
  }
}