
import { MoneyOperationUtils } from '@/helpers/money-operation-utils';
import { Account } from '@/models/account';
import { BudgetSummary } from '@/models/budget-summary';
import { Fund } from '@/models/fund';
import axios from 'axios';

export async function fetchBudgetSummary(): Promise<BudgetSummary> {
  const res = await axios.get<BudgetSummary>('budget');
  const budgetSummary = res?.data;
  return applyBudgetSettings(budgetSummary);
}

function applyBudgetSettings(budgetSummary: BudgetSummary): BudgetSummary {
  return {
    ...budgetSummary,
    operations: MoneyOperationUtils.sort(budgetSummary.operations.map(x => MoneyOperationUtils.parseFromResponse(x))),
    accounts: sortAccounts(budgetSummary.accounts, budgetSummary.userSettings),
    funds: sortFunds(budgetSummary.funds, budgetSummary.userSettings),
  };
}

function sortFunds(
  funds: Fund[],
  settings: { accountsOrder: string[], fundsOrder: string[] }
): Fund[] {
  const sorted: Fund[] = [];
  settings.fundsOrder.forEach(id => {
    const fund = funds.find(fund => fund.id === id);
    if (fund) {
      sorted.push(fund);
    }
  });
  const unsorted = funds.filter(fund => !settings.fundsOrder.find(x => x === fund.id));
  return [...unsorted, ...sorted];
}

function sortAccounts(
  accounts: Account[],
  settings: { accountsOrder: string[], fundsOrder: string[] }
): Account[] {
  const sorted: Account[] = [];
  settings.accountsOrder.forEach(id => {
    const account = accounts.find(fund => fund.id === id);
    if (account) {
      sorted.push(account);
    }
  });
  const unsorted = accounts.filter(account => !settings.accountsOrder.find(x => x === account.id));
  return [...unsorted, ...sorted];
}