import { IncomeAllocationRuleType } from './income-allocation-rule-type.enum';

export type IncomeAllocationRule = {
  id: number,
  value: number,
  fundId?: string,
  fundName?: string,
  type: IncomeAllocationRuleType
}
