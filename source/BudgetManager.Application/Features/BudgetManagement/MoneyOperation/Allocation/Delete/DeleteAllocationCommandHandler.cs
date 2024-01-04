namespace BudgetManager.Application.Features.BudgetManagement;

using AutoMapper;
using BudgetManager.Domain.Models;
using BudgetManager.Infrastructure;

public class DeleteAllocationCommandHandler(IUserBudgetRepository repo, IMapper map)
  : DeleteMOneyOperationCommandHandler<Allocation>(repo, map)
{
}
