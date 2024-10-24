namespace BudgetManager.Application.Features.BudgetManagement;

using AutoMapper;
using BudgetManager.Domain.Models;
using BudgetManager.Infrastructure;

public class DeleteIncomeDistributionTemplateCommandHandler(IUserBudgetRepository repo, IMapper map)
  : BudgetCommandHandler<DeleteIncomeDistributionTemplateCommand, Unit>(repo, map)
{
  public override Unit ModifyBudget(DeleteIncomeDistributionTemplateCommand command, Budget budget)
  {
    budget.RemoveIncomeDistributionTemplate(command.IncomeDistributionTemplateId);
    return Unit.Value;
  }
}
