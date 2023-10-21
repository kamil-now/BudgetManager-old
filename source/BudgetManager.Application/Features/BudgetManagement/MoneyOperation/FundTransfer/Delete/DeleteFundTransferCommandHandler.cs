namespace BudgetManager.Application.Features.BudgetManagement;

using AutoMapper;
using BudgetManager.Domain.Models;
using BudgetManager.Infrastructure;

public class DeleteFundTransferCommandHandler : DeleteMOneyOperationCommandHandler<FundTransfer>
{
  public DeleteFundTransferCommandHandler(IUserBudgetRepository repo, IMapper map) : base(repo, map)
  {
  }
}
