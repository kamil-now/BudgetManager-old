import { Account } from './account';
import { Fund } from './fund';

export type BudgetSummary = {
  funds: Fund[],
  accounts: Account[]
};