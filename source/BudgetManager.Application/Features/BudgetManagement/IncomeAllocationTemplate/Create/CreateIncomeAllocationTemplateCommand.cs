namespace BudgetManager.Application.Features.BudgetManagement;

public record CreateIncomeAllocationTemplateCommand(
  string UserId,
  string Name,
  string DefaultFundId,
  IEnumerable<IncomeAllocationRuleDto> Rules
) : IRequest<string>, IBudgetCommand;