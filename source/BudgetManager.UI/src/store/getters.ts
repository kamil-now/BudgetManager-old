import { AccountTransfer } from '@/models/account-transfer';
import { Allocation } from '@/models/allocation';
import { CurrencyExchange } from '@/models/currency-exchange';
import { Expense } from '@/models/expense';
import { FundTransfer } from '@/models/fund-transfer';
import { Income } from '@/models/income';
import { MoneyOperationType } from '@/models/money-operation-type.enum';
import { Account } from '@/models/account';
import { Balance } from '@/models/balance';
import { Fund } from '@/models/fund';
import { MoneyOperation } from '@/models/money-operation';
import { AppState } from './state';

export type AppGetters = {
  balance: (state: AppState) => Balance,
  unallocated: (state: AppState) => Balance,
  filteredOperations: (state: AppState) => MoneyOperation[],
  operations: (state: AppState) => MoneyOperation[],
  funds: (state: AppState) => Fund[],
  fundsNames: (state: AppState) => (string | undefined)[],
  accounts: (state: AppState) => Account[],
  accountsNames: (state: AppState) => (string | undefined)[],
  incomes: (state: AppState) => Income[],
  allocations: (state: AppState) => Allocation[],
  expenses: (state: AppState) => Expense[],
  currencyExchanges: (state: AppState) => CurrencyExchange[],
  fundTransfers: (state: AppState) => FundTransfer[],
  accountTransfers: (state: AppState) => AccountTransfer[],
};

export const APP_GETTERS: AppGetters = {
  balance: (state: AppState) => state.budget.balance,
  unallocated: (state: AppState) => state.budget.unallocated,
  filteredOperations: (state: AppState) => {
    let operations = [...state.budget.operations];
    if (state.operationsFilter.length > 1) {
      const filterValue = state.operationsFilter.toLowerCase();
      operations = state.budget.operations.filter(x => (x.title + x.accountName + x.fundName + x.targetFundName + x.targetAccountName).toLowerCase().includes(filterValue));
    }
    if (state.operationsTypeFilter !== MoneyOperationType.Undefined) {
      operations = operations.filter(x => x.type === state.operationsTypeFilter);
    }
    if (state.operationsDateFilter.length > 0) {
      operations = operations.filter(x => x.date === state.operationsDateFilter);
    }
    return operations;
  }, 
  operations: (state: AppState) => state.budget.operations, 
  funds: (state: AppState) => state.budget.funds.filter(x => !x.isDeleted),
  fundsNames: (state: AppState) => state.budget.funds.filter(x => !x.isDeleted).map(x => x.name),
  accounts: (state: AppState) => state.budget.accounts.filter(x => !x.isDeleted),
  accountsNames: (state: AppState) => state.budget.accounts.filter(x => !x.isDeleted).map(x => x.name),
  incomes: (state: AppState) => state.budget.operations
    .filter(x => x.type === MoneyOperationType.Income).map(x => x as Income),
  allocations: (state: AppState) => state.budget.operations
    .filter(x => x.type === MoneyOperationType.Allocation).map(x => x as Allocation),
  expenses: (state: AppState) => state.budget.operations
    .filter(x => x.type === MoneyOperationType.Expense).map(x => x as Expense),
  currencyExchanges: (state: AppState) => state.budget.operations
    .filter(x => x.type === MoneyOperationType.CurrencyExchange).map(x => x as CurrencyExchange),
  accountTransfers: (state: AppState) => state.budget.operations
    .filter(x => x.type === MoneyOperationType.AccountTransfer).map(x => x as AccountTransfer),
  fundTransfers: (state: AppState) => state.budget.operations
    .filter(x => x.type === MoneyOperationType.FundTransfer).map(x => x as FundTransfer),
};
