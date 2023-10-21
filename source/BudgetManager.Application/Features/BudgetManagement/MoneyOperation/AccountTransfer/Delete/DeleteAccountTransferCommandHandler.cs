namespace BudgetManager.Application.Features.BudgetManagement;

using AutoMapper;
using BudgetManager.Domain.Models;
using BudgetManager.Infrastructure;

public class DeleteAccountTransferCommandHandler : DeleteMOneyOperationCommandHandler<AccountTransfer>
{
  public DeleteAccountTransferCommandHandler(IUserBudgetRepository repo, IMapper map) : base(repo, map)
  {
  }
}
