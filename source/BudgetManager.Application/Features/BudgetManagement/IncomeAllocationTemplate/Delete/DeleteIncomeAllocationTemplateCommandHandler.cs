namespace BudgetManager.Application.Features.BudgetManagement;

using AutoMapper;
using BudgetManager.Domain.Models;
using BudgetManager.Infrastructure;

public class DeleteIncomeAllocationTemplateCommandHandler(IUserBudgetRepository repo, IMapper map)
  : BudgetCommandHandler<DeleteIncomeAllocationTemplateCommand, Unit>(repo, map)
{
  public override Unit ModifyBudget(DeleteIncomeAllocationTemplateCommand command, Budget budget)
  {
    budget.RemoveIncomeAllocationTemplate(command.IncomeAllocationTemplateId);
    return Unit.Value;
  }
}
