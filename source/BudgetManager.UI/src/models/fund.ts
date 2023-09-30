import { Balance } from './balance';

export type Fund = {
  id?: string,
  name?: string,
  balance: Balance,
  isDeleted: boolean
}