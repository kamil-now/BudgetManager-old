namespace BudgetManager.Application.Commands;

using AutoMapper;
using BudgetManager.Domain.Models;
using BudgetManager.Infrastructure;

public record UpdateAccountTransferCommand(
  string UserId,
  string OperationId,
  string Title,
  Money Value,
  string? Date,
  string? Description,
  string? AccountId,
  string? TargetAccountId
  ) : UpdateOperationCommand<AccountTransfer, AccountTransferDto>(UserId, OperationId);
public class UpdateAccountTransferCommandHandler : UpdateOperationCommandHandler<UpdateAccountTransferCommand, AccountTransfer, AccountTransferDto>
{
  public UpdateAccountTransferCommandHandler(IUserBudgetRepository repo, IMapper map)
  : base(repo, map)
  {
  }
  protected override void Update(AccountTransfer operation, UpdateAccountTransferCommand command)
    => operation.Update(
        command.AccountId,
        command.TargetAccountId,
        command.Title,
        command.Value,
        command.Date,
        command.Description
      );
}


public class UpdateAccountTransferCommandValidator : UpdateOperationCommandValidator<UpdateAccountTransferCommand>
{
  public UpdateAccountTransferCommandValidator(IUserBudgetRepository repository, AppConfig appConfig) : base(repository)
  {
    RuleFor(x => x.Title)
      .Must(title => title is null || title.Length <= appConfig.MaxTitleLength);

    RuleFor(x => x)
      .MustAsync(async (command, cancellation) =>
      {
        if (command.AccountId is null)
          return true;
        var budget = await repository.Get(command.UserId);
        return budget!.Accounts?.Any(x => x.Id == command.AccountId) ?? false;
      })
      .WithMessage(command => $"Source Account with id {command.AccountId} does not exist in the budget");

    RuleFor(x => x)
      .MustAsync(async (command, cancellation) =>
      {
        if (command.TargetAccountId is null)
          return true;
        var budget = await repository.Get(command.UserId);
        return budget!.Accounts?.Any(x => x.Id == command.TargetAccountId) ?? false;
      }).WithMessage(command => $"Target account with id {command.TargetAccountId} does not exist in the budget");
  }
}