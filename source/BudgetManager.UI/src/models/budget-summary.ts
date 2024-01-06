import { Account } from './account';
import { Balance } from './balance';
import { Fund } from './fund';
import { IncomeDistribution } from './income-distribution';
import { MoneyOperation } from './money-operation';

export type BudgetSummary = {
  userSettings: { accountsOrder: string[], fundsOrder: string[] },
  balance: Balance,
  unallocated: Balance,
  funds: Fund[],
  accounts: Account[],
  operations: MoneyOperation[],
  incomeDistributionTemplates: IncomeDistribution[]
};