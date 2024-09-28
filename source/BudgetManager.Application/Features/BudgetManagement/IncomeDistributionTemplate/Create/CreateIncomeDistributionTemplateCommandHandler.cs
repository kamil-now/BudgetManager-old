namespace BudgetManager.Application.Features.BudgetManagement;

using AutoMapper;
using BudgetManager.Domain.Models;
using BudgetManager.Infrastructure;

public class CreateIncomeDistributionTemplateCommandHandler(IUserBudgetRepository repo, IMapper map)
  : BudgetCommandHandler<CreateIncomeDistributionTemplateCommand, string>(repo, map)
{
  public override string ModifyBudget(CreateIncomeDistributionTemplateCommand command, Budget budget)
  {
    var id = budget.AddIncomeDistributionTemplate(
      command.Name,
      command.DefaultFundId,
      _mapper.Map<IEnumerable<IncomeDistributionRule>>(command.Rules)
      );
    return id;
  }
}
