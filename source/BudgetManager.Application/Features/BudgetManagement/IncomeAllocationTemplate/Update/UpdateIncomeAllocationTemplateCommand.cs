namespace BudgetManager.Application.Features.BudgetManagement;

public record UpdateIncomeAllocationTemplateCommand(
  string UserId,
  string IncomeAllocationTemplateId,
  string? Name,
  string? DefaultFundId,
  IEnumerable<IncomeAllocationRuleDto>? Rules
) : IRequest<IncomeAllocationTemplateDto>, IBudgetCommand;
