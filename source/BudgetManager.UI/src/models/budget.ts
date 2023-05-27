import { Account } from './account';
import { Fund } from './fund';

export type Budget = {
  defaultFundName: string,
  defaultCurrency: string,
  accounts: Account[],
  funds: Fund[]
}