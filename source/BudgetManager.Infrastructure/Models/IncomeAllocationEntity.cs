namespace BudgetManager.Infrastructure.Models;

public class IncomeAllocationRuleEntity
{
  public string? Id { get; set; }
  public int? Value { get; set; }
  public string? FundId { get; set; }
  public int? Type { get; set; }
  public DateTime CreatedDate { get; set; }
}

public class IncomeAllocationTemplateEntity
{
  public string? Id { get; set; }
  public string? Name { get; set; }
  public string? DefaultFundId { get; set; }
  public IEnumerable<IncomeAllocationRuleEntity>? Rules { get; set; }
  public DateTime CreatedDate { get; set; }
  public bool IsDeleted { get; set; }
}
