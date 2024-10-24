namespace BudgetManager.Application.Features.BudgetManagement;

using AutoMapper;

public class IncomeAllocationTemplatesListRequestHandler(IUserBudgetRepository repo, IMapper map)
  : BudgetRequestHandler<BudgetRequest<IncomeAllocationTemplateDto>, IEnumerable<IncomeAllocationTemplateDto>>(repo, map)
{
  public override IEnumerable<IncomeAllocationTemplateDto> Get(BudgetRequest<IncomeAllocationTemplateDto> request, Budget budget)
  => budget.IncomeAllocationTemplates.Select(_mapper.Map<IncomeAllocationTemplateDto>);
}
