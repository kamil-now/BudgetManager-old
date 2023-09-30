
import { MoneyOperationUtils } from '@/helpers/money-operation-utils';
import { Account } from '@/models/account';
import { BudgetSummary } from '@/models/budget-summary';
import { Fund } from '@/models/fund';
import { MoneyOperation } from '@/models/money-operation';
import axios from 'axios';

export function fetchBudgetSummary(): Promise<BudgetSummary> {
  return axios.get<BudgetSummary>('budget')
    .then(res => {
      const budgetSummary = res?.data;
      return applyBudgetSettings(budgetSummary);
    });
}

function applyBudgetSettings(budgetSummary: BudgetSummary): BudgetSummary {
  return {
    ...budgetSummary,
    operations: parseAndSortOperations(budgetSummary.operations),
    accounts: sortAccounts(budgetSummary.accounts, budgetSummary.userSettings),
    funds: sortFunds(budgetSummary.funds, budgetSummary.userSettings),
  };
}

function parseAndSortOperations<T extends MoneyOperation>(operations: T[]):T[] {
  return operations.map(x => MoneyOperationUtils.parseFromResponse(x)).sort((a, b) => {
    const byDate = new Date(b.date).valueOf() - new Date(a.date).valueOf();
    return byDate === 0 ? new Date(b.createdDate).valueOf() - new Date(a.createdDate).valueOf() : byDate;
  });
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