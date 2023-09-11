import { MoneyOperation } from './money-operation';

export type AccountTransfer = MoneyOperation & {
  targetAccountId: string,
  sourceAccountId: string,
}