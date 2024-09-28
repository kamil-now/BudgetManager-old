namespace BudgetManager.Application.Features.BudgetManagement;

public record UpdateIncomeDistributionTemplateCommand(
  string UserId,
  string IncomeDistributionTemplateId,
  string? Name,
  string? DefaultFundId,
  IEnumerable<IncomeDistributionRuleDto>? Rules
) : IRequest<IncomeDistributionTemplateDto>, IBudgetCommand;
