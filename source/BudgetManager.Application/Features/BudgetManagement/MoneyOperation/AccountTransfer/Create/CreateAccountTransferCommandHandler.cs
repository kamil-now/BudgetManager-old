namespace BudgetManager.Application.Features.BudgetManagement;

using AutoMapper;
using BudgetManager.Domain.Models;
using BudgetManager.Infrastructure;

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

    budget.AddOperation(
      new AccountTransfer(
        id,
        command.Title,
        command.Value,
        command.AccountId,
        command.TargetAccountId,
        command.Date,
        command.Description ?? string.Empty,
        DateTime.Now
        )
      );

    return id;
  }
}
