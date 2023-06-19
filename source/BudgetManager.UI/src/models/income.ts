import { MoneyOperation } from './money-operation';

export type Income = MoneyOperation & {
  accountId: string,
  fundId: string,
}