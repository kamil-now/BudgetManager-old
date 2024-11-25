namespace BudgetManager.Domain.Models;

public enum IncomeAllocationRuleType
{
  Undefined,
  Fixed,
  Percent,
}

public class IncomeAllocationRule(
  string id,
  int value,
  string fundId,
  IncomeAllocationRuleType type
)
{
  public string Id { get; private set; } = id;
  public int Value { get; private set; } = value;
  public string FundId { get; private set; } = fundId;
  public IncomeAllocationRuleType Type { get; private set; } = type;
  public void Update(int? value, string? fundId, IncomeAllocationRuleType? type)
  {
    if (value is not null)
    {
      Value = (int)value;
    }
    if (fundId is not null)
    {
      FundId = fundId;
    }
    if (type is not null)
    {
      Type = (IncomeAllocationRuleType)type;
    }
  }
}

public class IncomeAllocationTemplate(
  string id,
  string name,
  string defaultFundId,
  IEnumerable<IncomeAllocationRule> rules
  )
{
  public string Id { get; private set; } = id;
  public string Name { get; private set; } = name;

  public IEnumerable<IncomeAllocationRule> Rules { get; private set; } = rules;
  public string DefaultFundId { get; private set; } = defaultFundId;

  public void Update(string? name, string? defaultFundId, IEnumerable<IncomeAllocationRule>? rules)
  {
    if (name is not null)
    {
      Name = name;
    }
    if (defaultFundId is not null)
    {
      DefaultFundId = defaultFundId;
    }
    if (rules is not null)
    {
      foreach (var rule in rules)
      {
        var existing = Rules.FirstOrDefault(r => r.Id == rule.Id);
        if (existing is not null)
        {
          existing.Update(rule.Value, rule.FundId, rule.Type);
          Rules = Rules.Select(r => r.Id == rule.Id ? existing : r);
        }
        else
        {
          Rules = Rules.Append(rule);
        }
      }
      Rules = Rules.Where(x => rules.Any(r => r.Id == x.Id));
    }
  }
}
