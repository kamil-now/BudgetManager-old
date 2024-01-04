namespace BudgetManager.Application.Features.BudgetManagement;

using AutoMapper;
using BudgetManager.Domain.Models;
using BudgetManager.Infrastructure;

public class DeleteAccountTransferCommandHandler(IUserBudgetRepository repo, IMapper map)
  : DeleteMOneyOperationCommandHandler<AccountTransfer>(repo, map)
{
}
