namespace BudgetManager.Application.Commands;

using AutoMapper;
using BudgetManager.Domain.Models;
using BudgetManager.Infrastructure;

public record UpdateFundTransferCommand(
  string UserId,
  string OperationId,
  string Title,
  Money Value,
  string? Date,
  string? Description,
  string? SourceFundId,
  string? TargetFundId
  ) : UpdateOperationCommand<FundTransfer, FundTransferDto>(UserId, OperationId);
public class UpdateFundTransferCommandHandler : UpdateOperationCommandHandler<UpdateFundTransferCommand, FundTransfer, FundTransferDto>
{
  public UpdateFundTransferCommandHandler(IUserBudgetRepository repo, IMapper map)
  : base(repo, map)
  {
  }
  protected override void Update(FundTransfer operation, UpdateFundTransferCommand command)
    => operation.Update(
        command.SourceFundId,
        command.TargetFundId,
        command.Title,
        command.Value,
        command.Date,
        command.Description
      );
}


public class UpdateFundTransferCommandValidator : UpdateOperationCommandValidator<UpdateFundTransferCommand>
{
  public UpdateFundTransferCommandValidator(IUserBudgetRepository repository, AppConfig appConfig) : base(repository)
  {
    RuleFor(x => x.Title)
      .Must(title => title is null || title.Length <= appConfig.MaxTitleLength);

    RuleFor(x => x)
      .MustAsync(async (command, cancellation) =>
      {
        if (command.SourceFundId is null)
          return true;
        var budget = await repository.Get(command.UserId);
        return budget!.Funds?.Any(x => x.Id == command.SourceFundId) ?? false;
      })
      .WithMessage(command => $"Source Fund with id {command.SourceFundId} does not exist in the budget");

    RuleFor(x => x)
      .MustAsync(async (command, cancellation) =>
      {
        if (command.TargetFundId is null)
          return true;
        var budget = await repository.Get(command.UserId);
        return budget!.Funds?.Any(x => x.Id == command.TargetFundId) ?? false;
      }).WithMessage(command => $"Target fund with id {command.TargetFundId} does not exist in the budget");
  }
}