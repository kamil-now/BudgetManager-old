namespace BudgetManager.Application.Commands;

using AutoMapper;
using BudgetManager.Domain.Models;
using BudgetManager.Infrastructure;

public class DeleteFundTransferCommandHandler : DeleteOperationCommandHandler<FundTransfer>
{
  public DeleteFundTransferCommandHandler(IUserBudgetRepository repo, IMapper map) : base(repo, map)
  {
  }
}

public class DeleteFundTransferCommandValidator : UpdateOperationCommandValidator<DeleteOperationCommand<FundTransfer>>
{
  public DeleteFundTransferCommandValidator(IUserBudgetRepository repository) : base(repository)
  {
  }
}