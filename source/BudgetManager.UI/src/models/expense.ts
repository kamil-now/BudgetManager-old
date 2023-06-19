import { MoneyOperation } from './money-operation';

export type Expense = MoneyOperation & {
  accountId: string,
  fundId: string,
  isConfirmed: boolean,
}