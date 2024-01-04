namespace BudgetManager.Application.Features.BudgetManagement;

using AutoMapper;
using BudgetManager.Domain.Models;
using BudgetManager.Infrastructure;

public class CreateFundTransferCommandHandler(IUserBudgetRepository repo, IMapper map)
  : BudgetCommandHandler<CreateFundTransferCommand, string>(repo, map)
{
  public override string ModifyBudget(CreateFundTransferCommand command, Budget budget)
  {
    var id = Guid.NewGuid().ToString();

    budget.AddOperation(
      new FundTransfer(
        id,
        command.Title,
        command.Value,
        command.FundId,
        command.TargetFundId,
        command.Date,
        command.Description ?? string.Empty,
        DateTime.Now
        )
      );

    return id;
  }
}
