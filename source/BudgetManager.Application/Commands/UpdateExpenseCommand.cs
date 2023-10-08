namespace BudgetManager.Application.Commands;

using AutoMapper;
using BudgetManager.Domain.Models;
using BudgetManager.Infrastructure;

public record UpdateExpenseCommand(
  string UserId,
  string OperationId,
  string? Title,
  Money? Value,
  string? Date,
  string? AccountId,
  string? FundId,
  string? Description
  ) : UpdateOperationCommand<Expense, ExpenseDto>(UserId, OperationId);

public class UpdateExpenseCommandHandler : UpdateOperationCommandHandler<UpdateExpenseCommand, Expense, ExpenseDto>
{
  public UpdateExpenseCommandHandler(IUserBudgetRepository repo, IMapper map)
  : base(repo, map)
  {
  }

  protected override void Update(Expense operation, UpdateExpenseCommand command)
    => operation.Update(
        command.FundId,
        command.AccountId,
        command.Title,
        command.Value,
        command.Date,
        command.Description
      );

  protected override ExpenseDto CompleteDto(ExpenseDto dto, Budget budget)
    => dto with
    {
      Type = MoneyOperationType.Expense,
      AccountName = budget.Accounts.First(x => x.Id == dto.AccountId).Name,
      FundName = budget.Funds.First(x => x.Id == dto.FundId).Name,
    };
}

public class UpdateExpenseCommandValidator : UpdateOperationCommandValidator<UpdateExpenseCommand>
{
  public UpdateExpenseCommandValidator(IUserBudgetRepository repository, AppConfig config) : base(repository)
  {
    RuleFor(x => x.Title)
      .Must(title => title is null || title.Length <= config.MaxTitleLength);

    RuleFor(x => x.Value).ISO_4217_Currency(allowNull: true);

    RuleFor(x => x)
      .MustAsync(async (command, cancellation) =>
      {
        if (command.AccountId is null)
          return true;
        var budget = await repository.Get(command.UserId);
        return budget.Accounts?.Any(x => x.Id == command.AccountId) ?? false;
      }).WithMessage(command => $"Account with id {command.AccountId} does not exist in the budget");

    RuleFor(x => x)
      .MustAsync(async (command, cancellation) =>
      {
        if (command.FundId is null)
          return true;
        var budget = await repository.Get(command.UserId);
        return budget.Funds?.Any(x => x.Id == command.FundId) ?? false;
      }).WithMessage(command => $"Fund with id {command.FundId} does not exist in the budget");
  }
}