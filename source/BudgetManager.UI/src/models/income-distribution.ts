import { IncomeDistributionRule } from './income-distribution-rule';

export type IncomeDistribution = {
  rules: IncomeDistributionRule[],
  defaultFundId?: string,
  defaultFundName?: string
}
