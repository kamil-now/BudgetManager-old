import { Balance } from './balance';

export type Account = {
  id?: string,
  name?: string,
  balance: Balance,
  initialBalance: Balance,
  isDeleted: boolean
}