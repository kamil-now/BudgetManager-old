namespace BudgetManager.Application.Features.BudgetManagement;

using AutoMapper;
using BudgetManager.Domain.Models;
using BudgetManager.Infrastructure;

public class DeleteFundTransferCommandHandler(IUserBudgetRepository repo, IMapper map)
  : DeleteMoneyOperationCommandHandler<FundTransfer>(repo, map)
{
}
