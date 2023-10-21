namespace BudgetManager.Application.Features.BudgetManagement;

using AutoMapper;
using BudgetManager.Domain.Models;
using BudgetManager.Infrastructure;

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
