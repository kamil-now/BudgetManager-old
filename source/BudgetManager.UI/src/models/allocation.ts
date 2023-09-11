import { MoneyOperation } from './money-operation';

export type Allocation = MoneyOperation & {
  targetFundId: string,
}