import { IncomeAllocationRule } from './income-allocation-rule';

export type IncomeAllocation = {
  rules: IncomeAllocationRule[],
  defaultFundId?: string,
  defaultFundName?: string
}
