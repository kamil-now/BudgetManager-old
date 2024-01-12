import { IncomeDistributionRuleType } from './income-distribution-rule-type.enum';

export type IncomeDistributionRule = {
  id: number,
  value: number,
  fundId?: string,
  fundName?: string,
  type: IncomeDistributionRuleType
}
