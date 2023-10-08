import { Money } from './money';
import { MoneyOperationType } from './money-operation-type.enum';

export type MoneyOperation = {
  type: MoneyOperationType,
  id: string | undefined,
  createdDate: Date,
  title: string,
  value: Money,
  date: Date,
  description?: string,
  accountId?: string,
  accountName?: string,
  targetAccountId?: string,
  targetAccountName?: string,
  fundId?: string,
  fundName?: string,
  targetFundId?: string,
  targetFundName?: string,
  targetCurrency?: string,
  exchangeRate?: string
}
