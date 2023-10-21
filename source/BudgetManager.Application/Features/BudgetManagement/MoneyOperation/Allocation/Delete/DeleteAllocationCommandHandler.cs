namespace BudgetManager.Application.Features.BudgetManagement;

using AutoMapper;
using BudgetManager.Domain.Models;
using BudgetManager.Infrastructure;

public class DeleteAllocationCommandHandler : DeleteMOneyOperationCommandHandler<Allocation>
{
  public DeleteAllocationCommandHandler(IUserBudgetRepository repo, IMapper map) : base(repo, map)
  {
  }
}
