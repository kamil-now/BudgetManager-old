import { MoneyOperation } from './money-operation';

export type FundTransfer = MoneyOperation & {
  targetFundId: string,
  sourceFundId: string,
}