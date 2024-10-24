namespace BudgetManager.Application.Features.BudgetManagement;

using AutoMapper;
using BudgetManager.Domain.Models;
using BudgetManager.Infrastructure;

public class UpdateIncomeDistributionTemplateCommandHandler(IUserBudgetRepository repo, IMapper map)
  : BudgetCommandHandler<UpdateIncomeDistributionTemplateCommand, IncomeDistributionTemplateDto>(repo, map)
{
  public override IncomeDistributionTemplateDto ModifyBudget(UpdateIncomeDistributionTemplateCommand command, Budget budget)
  {
    var incomeDistributionTemplate = budget.UpdateIncomeDistributionTemplate(
      command.IncomeDistributionTemplateId,
      command.Name,
      command.DefaultFundId,
      _mapper.Map<IEnumerable<IncomeDistributionRule>>(command.Rules)
    );
    var dto = _mapper.Map<IncomeDistributionTemplateDto>(incomeDistributionTemplate);
    return dto;
  }
}
