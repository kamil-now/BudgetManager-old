import { IncomeDistributionRule } from './income-distribution-rule';

export type IncomeDistribution = {
  id?: string;
  rules: IncomeDistributionRule[],
  defaultFundId?: string,
  defaultFundName?: string
}
