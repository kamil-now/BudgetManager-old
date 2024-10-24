namespace BudgetManager.Application.Features.BudgetManagement;

using AutoMapper;
using BudgetManager.Domain.Models;
using BudgetManager.Infrastructure;

public class CreateIncomeAllocationTemplateCommandHandler(IUserBudgetRepository repo, IMapper map)
  : BudgetCommandHandler<CreateIncomeAllocationTemplateCommand, string>(repo, map)
{
  public override string ModifyBudget(CreateIncomeAllocationTemplateCommand command, Budget budget)
  {
    var id = budget.AddIncomeAllocationTemplate(
      command.Name,
      command.DefaultFundId,
      _mapper.Map<IEnumerable<IncomeAllocationRule>>(command.Rules)
      );
    return id;
  }
}
