
import { MoneyOperationUtils } from '@/helpers/money-operation-utils';
import { Account } from '@/models/account';
import { AccountTransfer } from '@/models/account-transfer';
import { Allocation } from '@/models/allocation';
import { Balance } from '@/models/balance';
import { CurrencyExchange } from '@/models/currency-exchange';
import { Expense } from '@/models/expense';
import { Fund } from '@/models/fund';
import { FundTransfer } from '@/models/fund-transfer';
import { Income } from '@/models/income';
import { MoneyOperation } from '@/models/money-operation';
import axios from 'axios';

export function fetchBudgetRequest(): Promise<{
  accounts: Account[],
  funds: Fund[],
  balance: {balance: Balance, unallocated: Balance},
  incomes: Income[],
  expenses: Expense[],
  allocations: Allocation[],
  fundTransfers: FundTransfer[],
  accountTransfers: AccountTransfer[],
  currencyExchanges: CurrencyExchange[],
} | null> {

  return axios.get<{balance: Balance, unallocated: Balance}>('/api/balance')
    .then(res => 
      Promise.all(
        [
          axios.get<Account[]>('api/accounts').then(res => res?.data),
          axios.get<Fund[]>('api/funds').then(res => res?.data),
          axios.get<Income[]>('api/incomes').then(res => res?.data),
          axios.get<Expense[]>('api/expenses').then(res => res?.data),
          axios.get<Allocation[]>('api/allocations').then(res => res?.data),
          axios.get<FundTransfer[]>('api/fund-transfers').then(res => res?.data),
          axios.get<AccountTransfer[]>('api/account-transfers').then(res => res?.data),
          axios.get<CurrencyExchange[]>('api/currency-exchanges').then(res => res?.data),
          axios.get<{ accountsOrder: string[], fundsOrder: string[] }>('api/user-settings')
            .then(res => res?.data, () => ({ accountsOrder: [], fundsOrder: [] }))
        ])
        .then(data => {
          if (data.some(x => !x)) {
            throw new Error('Failed to fetch budget data.');
          }
          const [accounts, funds, incomes, expenses, allocations, fundTransfers, accountTransfers, currencyExchanges, settings] = data;
          return {
            balance: res.data,
            incomes: parseAndSortOperations(incomes),
            expenses: parseAndSortOperations(expenses),
            accounts: sortAccounts(accounts, settings),
            funds: sortFunds(funds, settings),
            allocations: parseAndSortOperations(allocations),
            fundTransfers: parseAndSortOperations(fundTransfers),
            accountTransfers: parseAndSortOperations(accountTransfers),
            currencyExchanges: parseAndSortOperations(currencyExchanges),
          };})
    );
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