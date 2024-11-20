import { IncomeAllocationRuleType } from './income-allocation-rule-type.enum';

export type IncomeAllocationRule = {
  id: string,
  value: number,
  fundId?: string,
  fundName?: string,
  type: IncomeAllocationRuleType
}
