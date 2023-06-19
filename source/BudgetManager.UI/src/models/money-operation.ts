import { Money } from './money';

export type MoneyOperation = {
  id: string,
  createdDate: string,
  title: string,
  value: Money,
  date: string,
  description: string,
}
