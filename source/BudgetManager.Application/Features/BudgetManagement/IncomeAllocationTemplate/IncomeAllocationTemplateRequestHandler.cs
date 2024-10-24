namespace BudgetManager.Application.Features.BudgetManagement;

using AutoMapper;

public class IncomeAllocationTemplatesRequestHandler(IUserBudgetRepository repo, IMapper map)
  : BudgetRequestHandler<IncomeAllocationTemplateRequest, IncomeAllocationTemplateDto>(repo, map)
{
  public override IncomeAllocationTemplateDto Get(IncomeAllocationTemplateRequest request, Budget budget)
  {
    if (budget.IncomeAllocationTemplates.First(x => x.Id == request.IncomeAllocationTemplateId) is not IncomeAllocationTemplate template)
    {
      throw new Exception();
    }
    return _mapper.Map<IncomeAllocationTemplateDto>(template);
  }
}
