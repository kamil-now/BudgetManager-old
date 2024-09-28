namespace BudgetManager.Application.Features.BudgetManagement;

using AutoMapper;

public class IncomeDistributionTemplatesRequestHandler(IUserBudgetRepository repo, IMapper map)
  : BudgetRequestHandler<IncomeDistributionTemplateRequest, IncomeDistributionTemplateDto>(repo, map)
{
  public override IncomeDistributionTemplateDto Get(IncomeDistributionTemplateRequest request, Budget budget)
  {
    if (budget.IncomeDistributionTemplates.First(x => x.Id == request.IncomeDistributionTemplateId) is not IncomeDistributionTemplate template)
    {
      throw new Exception();
    }
    return _mapper.Map<IncomeDistributionTemplateDto>(template);
  }
}
