namespace BudgetManager.Application.Features.BudgetManagement;

public record IncomeAllocationRuleDto(
  string? Id = null,
  int? Value = null,
  string? FundId = null,
  string? FundName = null,
  int? Type = null
);

public record IncomeAllocationTemplateDto(
  string? Id = null,
  string? DefaultFundId = null,
  string? DefaultFundName = null,
  IEnumerable<IncomeAllocationRuleDto>? Rules = null
);