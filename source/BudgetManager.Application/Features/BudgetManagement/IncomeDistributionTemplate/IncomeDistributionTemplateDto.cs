namespace BudgetManager.Application.Features.BudgetManagement;

public record IncomeDistributionRuleDto(
  string? Id = null,
  int? Value = null,
  string? FundId = null,
  string? FundName = null,
  int? Type = null
);

public record IncomeDistributionTemplateDto(
  string? Id = null,
  string? DefaultFundId = null,
  string? DefaultFundName = null,
  IEnumerable<IncomeDistributionRuleDto>? Rules = null
);