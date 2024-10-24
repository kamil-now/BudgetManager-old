namespace BudgetManager.Application.Features.BudgetManagement;

using AutoMapper;

public class IncomeDistributionTemplatesListRequestHandler(IUserBudgetRepository repo, IMapper map)
  : BudgetRequestHandler<BudgetRequest<IncomeDistributionTemplateDto>, IEnumerable<IncomeDistributionTemplateDto>>(repo, map)
{
  public override IEnumerable<IncomeDistributionTemplateDto> Get(BudgetRequest<IncomeDistributionTemplateDto> request, Budget budget)
  => budget.IncomeDistributionTemplates.Select(_mapper.Map<IncomeDistributionTemplateDto>);
}
