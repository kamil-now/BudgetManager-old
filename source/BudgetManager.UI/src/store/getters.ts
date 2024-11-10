import currencies from '@/assets/currencies.json';
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
import { MoneyOperationType } from '@/models/money-operation-type.enum';
import { AppState } from './state';
import { IncomeAllocation } from '@/models/income-allocation';

export type AppGetters = {
  balance: (state: AppState) => Balance,
  unallocated: (state: AppState) => Balance,
  isFilteredByTypeOrContent: (state: AppState) => boolean,
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
  lastUsedAccount: (state: AppState) => Account | undefined,
  lastUsedFund: (state: AppState) => Fund | undefined,
  lastOperation: (state: AppState) => (type: MoneyOperationType) => MoneyOperation | undefined,
  incomeAllocationTemplates: (state: AppState) => IncomeAllocation[]
};

export const APP_GETTERS: AppGetters = {
  balance: (state: AppState) => state.budget.balance,
  unallocated: (state: AppState) => state.budget.unallocated,
  isFilteredByTypeOrContent: (state: AppState) => !!state.operationsTypeFilter || !!state.operationsContentFilter,
  filteredOperations: (state: AppState) => {
    let operations = [...state.budget.operations];
    if (state.operationsContentFilter.length > 1) {
      const filterValue = state.operationsContentFilter.toLowerCase();
      const valueFilter = Number(state.operationsContentFilter);
      if (valueFilter) {
        operations = state.budget.operations.filter(x => x.value.amount === valueFilter);
      } else if (
        filterValue.length === 3
        && Object.keys(currencies)
          .map(x => x.toLowerCase())
          .includes(filterValue)) {
        operations = state.budget.operations.filter(x => x.value.currency.toLowerCase().includes(filterValue));
      } else {
        operations = state.budget.operations.filter(x => (x.title + x.accountName + x.fundName + x.targetFundName + x.targetAccountName).toLowerCase().includes(filterValue));
      }
    }
    if (state.operationsTypeFilter !== MoneyOperationType.Undefined) {
      operations = operations.filter(x => x.type === state.operationsTypeFilter);
    }
    if (state.operationsDateFromFilter.length > 0) {
      operations = operations.filter(x => x.date >= state.operationsDateFromFilter);
    }
    if (state.operationsDateToFilter.length > 0) {
      operations = operations.filter(x => x.date <= state.operationsDateToFilter);
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
  lastUsedAccount: (state: AppState) => {
    const lastOperationWithAccount = state.budget.operations.find(x => !!x.accountId || !!x.targetAccountId);
    if (lastOperationWithAccount) {
      const account = state.budget.accounts.find(x =>
        !x.isDeleted &&
        (lastOperationWithAccount.accountId && x.id === lastOperationWithAccount.accountId)
        || (lastOperationWithAccount.targetAccountId && x.id === lastOperationWithAccount.targetAccountId)
      );
      if (account) {
        return account;
      }
    }
    const accountWithBalance = state.budget.accounts.find(x => !!x.id && Object.keys(x.balance).length > 0);
    return accountWithBalance;
  },
  lastUsedFund: (state: AppState) => {
    const lastOperationWithFund = state.budget.operations.find(x => !!x.fundId || !!x.targetFundId);
    if (lastOperationWithFund) {
      const fund = state.budget.funds.find(x =>
        !x.isDeleted &&
        (lastOperationWithFund.fundId && x.id === lastOperationWithFund.fundId)
        || (lastOperationWithFund.targetFundId && x.id === lastOperationWithFund.targetFundId)
      );
      if (fund) {
        return fund;
      }
    }
    const fundWithBalance = state.budget.funds.find(x => !!x.id && Object.keys(x.balance).length > 0);
    return fundWithBalance;
  },
  lastOperation: (state: AppState) => (type: MoneyOperationType) => {
    const operationsOfType = state.budget.operations.filter(x => x.type === type);
    return operationsOfType ? operationsOfType[0] : undefined;
  },
  incomeAllocationTemplates: (state: AppState) => state.budget.incomeAllocationTemplates
};
