namespace BudgetManager.Application.Commands;

using AutoMapper;
using BudgetManager.Domain.Models;
using BudgetManager.Infrastructure;

public class DeleteAllocationCommandHandler : DeleteOperationCommandHandler<Allocation>
{
  public DeleteAllocationCommandHandler(IUserBudgetRepository repo, IMapper map) : base(repo, map)
  {
  }
}

public class DeleteAllocationCommandValidator : AllocationCommandValidator<DeleteOperationCommand<Allocation>>
{
  public DeleteAllocationCommandValidator(IUserBudgetRepository repository) : base(repository)
  {
  }
}