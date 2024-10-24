namespace BudgetManager.Infrastructure.Models;

public class IncomeDistributionRuleEntity
{
  public string? Id { get; set; }
  public int? Value { get; set; }
  public string? FundId { get; set; }
  public int? Type { get; set; }
  public DateTime CreatedDate { get; set; }
}

public class IncomeDistributionTemplateEntity
{
  public string? Id { get; set; }
  public string? Name { get; set; }
  public string? DefaultFundId { get; set; }
  public IEnumerable<IncomeDistributionRuleEntity>? Rules { get; set; }
  public DateTime CreatedDate { get; set; }
  public bool IsDeleted { get; set; }
}
