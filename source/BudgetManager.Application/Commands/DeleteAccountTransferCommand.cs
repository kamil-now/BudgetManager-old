namespace BudgetManager.Application.Commands;

using AutoMapper;
using BudgetManager.Domain.Models;
using BudgetManager.Infrastructure;

public class DeleteAccountTransferCommandHandler : DeleteOperationCommandHandler<AccountTransfer>
{
  public DeleteAccountTransferCommandHandler(IUserBudgetRepository repo, IMapper map) : base(repo, map)
  {
  }
}

public class DeleteAccountTransferCommandValidator : UpdateOperationCommandValidator<DeleteOperationCommand<AccountTransfer>>
{
  public DeleteAccountTransferCommandValidator(IUserBudgetRepository repository) : base(repository)
  {
  }
}