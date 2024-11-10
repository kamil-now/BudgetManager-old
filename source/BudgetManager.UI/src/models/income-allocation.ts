import { IncomeAllocationRule } from './income-allocation-rule';

export type IncomeAllocation = {
  id: string;
  name: string;
  rules: IncomeAllocationRule[],
  defaultFundId?: string,
  defaultFundName?: string
}
