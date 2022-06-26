namespace BudgetManager.Application.Commands;

using AutoMapper;
using BudgetManager.Domain.Models;
using BudgetManager.Infrastructure;

public class DeleteIncomeCommandHandler : DeleteOperationCommandHandler<Income>
{
  public DeleteIncomeCommandHandler(IUserBudgetRepository repo, IMapper map) : base(repo, map)
  {
  }
}

public class DeleteIncomeCommandValidator : IncomeCommandValidator<DeleteOperationCommand<Income>>
{
  public DeleteIncomeCommandValidator(IUserBudgetRepository repository) : base(repository)
  {
  }
}