import { Money } from './money';

export type Account = {
  id?: string,
  name?: string,
  balance: Money,
  initialBalance: Money,
}