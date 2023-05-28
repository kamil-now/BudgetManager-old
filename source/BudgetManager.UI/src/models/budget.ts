import { Account } from './account';
import { Balance } from './balance';
import { Fund } from './fund';

export type Budget = {
  balance?: Balance,
  defaultFundName: string,
  defaultCurrency: string,
  accounts: Account[],
  funds: Fund[]
}