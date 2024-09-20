import { IncomeDistributionRuleType } from './income-distribution-rule-type.enum';

export type IncomeDistributionRule = {
  id: string,
  value: number,
  fundId?: string,
  fundName?: string,
  type: IncomeDistributionRuleType
}
