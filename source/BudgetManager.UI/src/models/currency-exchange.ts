import { MoneyOperation } from './money-operation';

export type CurrencyExchange = MoneyOperation & {
  accountId: string,
  targetCurrency: string,
  exchangeRate: number
}