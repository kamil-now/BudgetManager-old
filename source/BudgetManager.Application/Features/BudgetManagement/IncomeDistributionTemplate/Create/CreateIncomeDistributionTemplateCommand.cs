namespace BudgetManager.Application.Features.BudgetManagement;

public record CreateIncomeDistributionTemplateCommand(
  string UserId,
  string Name,
  string DefaultFundId,
  IEnumerable<IncomeDistributionRuleDto> Rules
) : IRequest<string>, IBudgetCommand;