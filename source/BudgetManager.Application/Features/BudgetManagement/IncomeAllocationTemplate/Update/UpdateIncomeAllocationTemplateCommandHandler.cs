namespace BudgetManager.Application.Features.BudgetManagement;

using AutoMapper;
using BudgetManager.Domain.Models;
using BudgetManager.Infrastructure;

public class UpdateIncomeAllocationTemplateCommandHandler(IUserBudgetRepository repo, IMapper map)
  : BudgetCommandHandler<UpdateIncomeAllocationTemplateCommand, IncomeAllocationTemplateDto>(repo, map)
{
  public override IncomeAllocationTemplateDto ModifyBudget(UpdateIncomeAllocationTemplateCommand command, Budget budget)
  {
    var incomeAllocationTemplate = budget.UpdateIncomeAllocationTemplate(
      command.IncomeAllocationTemplateId,
      command.Name,
      command.DefaultFundId,
      _mapper.Map<IEnumerable<IncomeAllocationRule>>(command.Rules)
    );
    var dto = _mapper.Map<IncomeAllocationTemplateDto>(incomeAllocationTemplate);
    return dto;
  }
}
