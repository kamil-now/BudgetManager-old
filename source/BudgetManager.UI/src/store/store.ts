import { createAccountTransferRequest, deleteAccountTransferRequest, getAccountTransferRequest, updateAccountTransferRequest } from '@/api/account-transfer-requests';
import { createAllocationRequest, deleteAllocationRequest, getAllocationRequest, updateAllocationRequest } from '@/api/allocation-requests';
import { getBalanceRequest } from '@/api/balance-requests';
import { createCurrencyExchangeRequest, deleteCurrencyExchangeRequest, getCurrencyExchangeRequest, updateCurrencyExchangeRequest } from '@/api/currency-exchange-requests';
import { createExpenseRequest, deleteExpenseRequest, getExpenseRequest, updateExpenseRequest } from '@/api/expense-requests';
import { createFundTransferRequest, deleteFundTransferRequest, getFundTransferRequest, updateFundTransferRequest } from '@/api/fund-transfer-requests';
import { createIncomeRequest, deleteIncomeRequest, getIncomeRequest, updateIncomeRequest } from '@/api/income-requests';
import { Account } from '@/models/account';
import { AccountTransfer } from '@/models/account-transfer';
import { Allocation } from '@/models/allocation';
import { Balance } from '@/models/balance';
import { BudgetSummary } from '@/models/budget-summary';
import { CurrencyExchange } from '@/models/currency-exchange';
import { Expense } from '@/models/expense';
import { Fund } from '@/models/fund';
import { FundTransfer } from '@/models/fund-transfer';
import { Income } from '@/models/income';
import { MoneyOperation } from '@/models/money-operation';
import { MoneyOperationType } from '@/models/money-operation-type.enum';
import axios from 'axios';
import { DefineStoreOptions, Store, defineStore } from 'pinia';
import { createAccountRequest, deleteAccountRequest, getAccountRequest, updateAccountRequest } from '../api/account-requests';
import { fetchBudgetSummary } from '../api/fetch-budget-request';
import { createFundRequest, deleteFundRequest, getFundRequest, updateFundRequest } from '../api/fund-requests';
import { MoneyOperationUtils } from '@/helpers/money-operation-utils';

export type AppState = {
  isLoading: boolean;
  isLoggedIn: boolean;
  isNewUser: boolean,
  budget: BudgetSummary,
  operationsFilter: string,
  operationsTypeFilter: MoneyOperationType,
  operationsDateFilter: string
};
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
export type AppActions = {
  createBudget(): void;
  updateUserSettings(): void;
  reorderFunds(oldIndex: number, newIndex: number): void;
  reorderAccounts(oldIndex: number, newIndex: number): void;
  setLoggedIn(value: boolean): void;
  fetchBudget(): void;

  createNewAccount(account: Account): void,
  updateAccount(account: Account): void;
  deleteAccount(accountId: string): void;

  createNewFund(fund: Fund): void,
  updateFund(fund: Fund): void;
  deleteFund(fundId: string): void;
  
  createNewIncome(income: Income): void,
  updateIncome(income: Income): void;
  deleteIncome(incomeId: string): void;

  createNewExpense(expense: Expense): void,
  updateExpense(expense: Expense): void;
  deleteExpense(expenseId: string): void;

  createNewAllocation(allocation: Allocation): void,
  updateAllocation(allocation: Allocation): void;
  deleteAllocation(allocationId: string): void;

  createNewFundTransfer(fundTransfer: FundTransfer): void,
  updateFundTransfer(fundTransfer: FundTransfer): void;
  deleteFundTransfer(fundTransferId: string): void;
  
  createNewAccountTransfer(accountTransfer: AccountTransfer): void,
  updateAccountTransfer(accountTransfer: AccountTransfer): void;
  deleteAccountTransfer(accountTransferId: string): void;

  createNewCurrencyExchange(currencyExchange: CurrencyExchange): void,
  updateCurrencyExchange(currencyExchange: CurrencyExchange): void;
  deleteCurrencyExchange(currencyExchangeId: string): void;
};
export type AppStore = Store<string, AppState, AppGetters, AppActions>;

export const getInitialAppState: () => AppState = () => ({
  isLoading: true,
  isLoggedIn: false,
  isNewUser: true,
  budget: {
    userSettings: { accountsOrder: [], fundsOrder: [] },
    balance: {},
    unallocated: {},
    funds: [],
    accounts: [],
    operations: []
  },
  operationsFilter: '',
  operationsTypeFilter: MoneyOperationType.Undefined,
  operationsDateFilter: ''
});

export const APP_STORE: DefineStoreOptions<
  string,
  AppState,
  AppGetters,
  AppActions
> = {
  id: 'app',
  state: () => getInitialAppState(),
  getters: {
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
  },
  actions: {
    async setLoggedIn(value: boolean) {
      this.isLoggedIn = value;
      this.fetchBudget();
    },
    async fetchBudget() {
      await Utils.runAsyncOperation(this, () =>
        fetchBudgetSummary()
          .then(res => {
            if (res !== null) {
              this.budget = res;
              this.isNewUser = false;
            } else {
              this.isNewUser = true;
            }
          },
          (error) => {
            console.error(error);
          })
      );
    },
    async createBudget() {
      await Utils.runAsyncOperation(this, () => axios.post<void>('budget'));
    },
    async reorderAccounts(oldIndex: number, newIndex: number) {
      const account = this.budget.accounts.splice(oldIndex, 1)[0];
      this.budget.accounts.splice(newIndex, 0, account);
      this.updateUserSettings();

    },
    async reorderFunds(oldIndex: number, newIndex: number) {
      oldIndex = this.budget.funds.indexOf(this.funds[oldIndex]);
      newIndex = this.budget.funds.indexOf(this.funds[newIndex]);
      const fund = this.budget.funds.splice(oldIndex, 1)[0];
      this.budget.funds.splice(newIndex, 0, fund);
      this.updateUserSettings();
    },
    async updateUserSettings() {
      await axios.put('user-settings', { 
        accountsOrder: this.budget.accounts.filter(x => !x.isDeleted).map(x => x.id),
        fundsOrder: this.budget.funds.filter(x => !x.isDeleted).map(x => x.id)
      });
    },
    
    async createNewAccount(account: Account) {
      await Utils.runAsyncOperation(this, async (state) => {
        const id = await createAccountRequest(account);
        const fromResponse = await getAccountRequest(id);
        state.budget.accounts.unshift(fromResponse);
        this.updateUserSettings();
      });
    },
    async updateAccount(account: Account) {
      await Utils.runAsyncOperation(this, (state) => 
        updateAccountRequest(account)
          .then(async account => {
            const fromState = state.budget.accounts.find(x => x.id === account.id);
            if (!fromState || !fromState.id) {
              throw new Error('Invalid operation - account does not exist.');
            }
            const index = state.budget.accounts.indexOf(fromState);
            const res = await getAccountRequest(fromState.id);
            state.budget.accounts[index] = res;
          })
      );
    },
    async deleteAccount(accountId: string) {
      await Utils.runAsyncOperation(this, (state) => 
        deleteAccountRequest(accountId)
          .then(() => state.budget.accounts = state.budget.accounts.filter(x => x.id !== accountId))
      );
    },

    async createNewFund(
      fund: Fund,
    ) {
      await Utils.runAsyncOperation(this, (state) => 
        createFundRequest(fund)
          .then(id => {
            state.budget.funds.unshift({ ...fund, id });
            this.updateUserSettings();
          })
      );
    },
    async updateFund(fund: Fund) {
      await Utils.runAsyncOperation(this, (state) => 
        updateFundRequest(fund)
          .then(() => {
            const fromState = state.budget.funds.find(x => x.id === fund.id);
            if (!fromState) {
              throw new Error('Invalid operation - fund does not exist.');
            }
            const index = state.budget.funds.indexOf(fromState);
            state.budget.funds[index] = fund; 
          })
      );
    },
    async deleteFund(fundId: string) {
      await Utils.runAsyncOperation(this, (state) => 
        deleteFundRequest(fundId)
          .then(() => state.budget.funds = state.budget.funds.filter(x => x.id !== fundId))
      );
    },

    async createNewExpense(
      expense: Expense,
    ) {
      await Utils.runAsyncOperation(this, async (state) => {
        const id = await createExpenseRequest(expense); 
        const fromResponse = await getExpenseRequest(id);
        state.budget.operations.unshift(fromResponse);
        MoneyOperationUtils.sort(state.budget.operations);

        await Utils.reloadAccount(this, expense.accountId);
        await Utils.reloadFund(this, expense.fundId);
        await Utils.reloadBalance(this);
      });
    },
    async updateExpense(expense: Expense) {
      await Utils.runAsyncOperation(this, async (state) => {
        const fromResponse = await updateExpenseRequest(expense);
        const fromState = state.budget.operations.find(x => x.id === expense.id);
        if (!fromState) {
          throw new Error('Invalid operation - expense does not exist.');
        }
        const index = state.budget.operations.indexOf(fromState);
        state.budget.operations[index] = fromResponse;
        MoneyOperationUtils.sort(state.budget.operations);
            
        await Utils.reloadAccount(this, fromResponse.accountId);
        await Utils.reloadFund(this, fromResponse.fundId);
        await Utils.reloadBalance(this);
      }
      );
    },
    async deleteExpense(expenseId: string) {
      await Utils.runAsyncOperation(this, (state) => 
        deleteExpenseRequest(expenseId)
          .then(async () => {
            const fromState = this.budget.operations.find(x => x.id === expenseId);
            if (!fromState || !fromState.accountId || !fromState.fundId) {
              throw new Error('Invalid operation - expense data is invalid.');
            }
            state.budget.operations = state.budget.operations.filter(x => x.id !== fromState.id);
            await Utils.reloadAccount(this, fromState.accountId);
            await Utils.reloadFund(this, fromState.fundId);
            await Utils.reloadBalance(this);
          })
      );
    },

    async createNewIncome(
      income: Income,
    ) {
      await Utils.runAsyncOperation(this, async (state) => {
        const id = await createIncomeRequest(income); 
        const fromResponse = await getIncomeRequest(id);
        state.budget.operations.unshift(fromResponse);
        MoneyOperationUtils.sort(state.budget.operations);

        await Utils.reloadAccount(this, income.accountId);
        await Utils.reloadBalance(this);
      });
    },
    async updateIncome(income: Income) {
      await Utils.runAsyncOperation(this, (state) => 
        updateIncomeRequest(income)
          .then(async income => {
            const fromState = state.budget.operations.find(x => x.id === income.id);
            if (!fromState) {
              throw new Error('Invalid operation - income does not exist.');
            }
            const index = state.budget.operations.indexOf(fromState);
            state.budget.operations[index] = income;
            MoneyOperationUtils.sort(state.budget.operations);

            await Utils.reloadAccount(this, income.accountId);
            await Utils.reloadBalance(this);
          })
      );
    },
    async deleteIncome(incomeId: string) {
      await Utils.runAsyncOperation(this, (state) => 
        deleteIncomeRequest(incomeId)
          .then(async () => {
            const fromState = this.budget.operations.find(x => x.id === incomeId);
            if (!fromState || !fromState.accountId) {
              throw new Error('Invalid operation - income data is invalid.');
            }
            state.budget.operations = state.budget.operations.filter(x => x.id !== fromState.id);
            await Utils.reloadAccount(this, fromState.accountId);
            await Utils.reloadBalance(this);
          })
      );
    },
    async createNewAllocation(
      allocation: Allocation,
    ) {
      await Utils.runAsyncOperation(this, async (state) => 
      {
        const id = await createAllocationRequest(allocation); 
        const fromResponse = await getAllocationRequest(id);
        state.budget.operations.unshift(fromResponse);
        MoneyOperationUtils.sort(state.budget.operations);

        await Utils.reloadFund(this, allocation.targetFundId);
        await Utils.reloadBalance(this);
      });
    },
    async updateAllocation(allocation: Allocation) {
      await Utils.runAsyncOperation(this, (state) => 
        updateAllocationRequest(allocation)
          .then(async allocation => {
            const fromState = state.budget.operations.find(x => x.id === allocation.id);
            if (!fromState) {
              throw new Error('Invalid operation - allocation does not exist.');
            }
            const index = state.budget.operations.indexOf(fromState);
            state.budget.operations[index] = allocation; 
            MoneyOperationUtils.sort(state.budget.operations);

            await Utils.reloadFund(this, allocation.targetFundId);
            await Utils.reloadBalance(this);
          })
      );
    },
    async deleteAllocation(allocationId: string) {
      await Utils.runAsyncOperation(this, (state) => 
        deleteAllocationRequest(allocationId)
          .then(async () => {
            const fromState = this.budget.operations.find(x => x.id === allocationId);
            if (!fromState || !fromState.fundId || !fromState.targetFundId) {
              throw new Error('Invalid operation - allocation data is invalid.');
            }
            state.budget.operations = state.budget.operations.filter(x => x.id !== fromState.id);
            await Utils.reloadFund(this, fromState.fundId);
            await Utils.reloadFund(this, fromState.targetFundId);
            await Utils.reloadBalance(this);
          })
      );
    },
    async createNewFundTransfer(
      fundTransfer: FundTransfer,
    ) {
      await Utils.runAsyncOperation(this, async (state) => {
        const id = await createFundTransferRequest(fundTransfer); 
        const fromResponse = await getFundTransferRequest(id);
        state.budget.operations.unshift(fromResponse);
        MoneyOperationUtils.sort(state.budget.operations);
        await Utils.reloadFund(this, fundTransfer.fundId);
        await Utils.reloadFund(this, fundTransfer.targetFundId);
        await Utils.reloadBalance(this);
      });
    },
    async updateFundTransfer(fundTransfer: FundTransfer) {
      await Utils.runAsyncOperation(this, (state) => 
        updateFundTransferRequest(fundTransfer)
          .then(async fundTransfer => {
            const fromState = state.budget.operations.find(x => x.id === fundTransfer.id);
            if (!fromState) {
              throw new Error('Invalid operation - fund transfer does not exist.');
            }
            const index = state.budget.operations.indexOf(fromState);
            state.budget.operations[index] = fundTransfer; 
            MoneyOperationUtils.sort(state.budget.operations);
            
            await Utils.reloadFund(this, fundTransfer.fundId);
            await Utils.reloadFund(this, fundTransfer.targetFundId);
            await Utils.reloadBalance(this);
          })
      );
    },
    async deleteFundTransfer(fundTransferId: string) {
      await Utils.runAsyncOperation(this, (state) => 
        deleteFundTransferRequest(fundTransferId)
          .then(async () => {
            const fromState = this.budget.operations.find(x => x.id === fundTransferId);
            if (!fromState || !fromState.fundId || !fromState.targetFundId) {
              throw new Error('Invalid operation - fund transfer data is invalid.');
            }
            state.budget.operations = state.budget.operations.filter(x => x.id !== fromState.id);
            await Utils.reloadFund(this, fromState.fundId);
            await Utils.reloadFund(this, fromState.targetFundId);
            await Utils.reloadBalance(this);
          })
      );
    },
    async createNewAccountTransfer(
      accountTransfer: AccountTransfer,
    ) {
      await Utils.runAsyncOperation(this, async (state) => {
        const id = await createAccountTransferRequest(accountTransfer); 
        const fromResponse = await getAccountTransferRequest(id);
        state.budget.operations.unshift(fromResponse);
        MoneyOperationUtils.sort(state.budget.operations);
        await Utils.reloadAccount(this, accountTransfer.accountId);
        await Utils.reloadAccount(this, accountTransfer.targetAccountId);
        await Utils.reloadBalance(this);
      });
    },
    async updateAccountTransfer(accountTransfer: AccountTransfer) {
      await Utils.runAsyncOperation(this, (state) => 
        updateAccountTransferRequest(accountTransfer)
          .then(async accountTransfer => {
            const fromState = state.budget.operations.find(x => x.id === accountTransfer.id);
            if (!fromState) {
              throw new Error('Invalid operation - account transfer does not exist.');
            }
            const index = state.budget.operations.indexOf(fromState);
            state.budget.operations[index] = accountTransfer; 
            MoneyOperationUtils.sort(state.budget.operations);

            await Utils.reloadAccount(this, accountTransfer.accountId);
            await Utils.reloadAccount(this, accountTransfer.targetAccountId);
            await Utils.reloadBalance(this);
          })
      );
    },
    async deleteAccountTransfer(accountTransferId: string) {
      await Utils.runAsyncOperation(this, (state) => 
        deleteAccountTransferRequest(accountTransferId)
          .then(async () => {
            const fromState = this.budget.operations.find(x => x.id === accountTransferId);
            if (!fromState || !fromState.accountId || !fromState.targetAccountId) {
              throw new Error('Invalid operation - account transfer data is invalid.');
            }
            state.budget.operations = state.budget.operations.filter(x => x.id !== fromState.id);
            await Utils.reloadAccount(this, fromState.accountId);
            await Utils.reloadAccount(this, fromState.targetAccountId);
            await Utils.reloadBalance(this);
          })
      );
    },
    async createNewCurrencyExchange(
      currencyExchange: CurrencyExchange,
    ) {
      await Utils.runAsyncOperation(this, async (state) => {
        const id = await createCurrencyExchangeRequest(currencyExchange); 
        const fromResponse = await getCurrencyExchangeRequest(id);
        state.budget.operations.unshift(fromResponse);
        MoneyOperationUtils.sort(state.budget.operations);

        await Utils.reloadAccount(this, currencyExchange.accountId);
        await Utils.reloadBalance(this);
      });
    },
    async updateCurrencyExchange(currencyExchange: CurrencyExchange) {
      await Utils.runAsyncOperation(this, (state) => 
        updateCurrencyExchangeRequest(currencyExchange)
          .then(async currencyExchange => {
            const fromState = state.budget.operations.find(x => x.id === currencyExchange.id);
            if (!fromState) {
              throw new Error('Invalid operation - currency exchange does not exist.');
            }
            const index = state.budget.operations.indexOf(fromState);
            state.budget.operations[index] = currencyExchange; 
            MoneyOperationUtils.sort(state.budget.operations);
        
            await Utils.reloadAccount(this, currencyExchange.accountId);
            await Utils.reloadBalance(this);
          })
      );
    },
    async deleteCurrencyExchange(currencyExchangeId: string) {
      await Utils.runAsyncOperation(this, (state) => 
        deleteCurrencyExchangeRequest(currencyExchangeId)
          .then(async () => {
            const fromState = this.budget.operations.find(x => x.id === currencyExchangeId);
            if (!fromState || !fromState.accountId) {
              throw new Error('Invalid operation - currency exchange data is invalid.');
            }
            state.budget.operations = state.budget.operations.filter(x => x.id !== fromState.id);
            await Utils.reloadAccount(this, fromState.accountId);
            await Utils.reloadBalance(this);
          })
      );
    },
  }
};

export const useAppStore = defineStore<
  string,
  AppState,
  AppGetters,
  AppActions
>(APP_STORE);

class Utils {
  static async runAsyncOperation(
    state: AppState,
    op: (state: AppState) => Promise<unknown>
  ): Promise<void> {
    state.isLoading = true;
    try {
      await op(state);
    } catch (error) {
      // TODO console.error(error);
    } finally {
      state.isLoading = false;
    }
  }

  static async reloadBalance(state: AppState): Promise<void> {
    const budgetBalance = await getBalanceRequest();
    state.budget.balance = budgetBalance.balance;
    state.budget.unallocated = budgetBalance.unallocated;
  }

  static async reloadAccount(state: AppState, accountId: string): Promise<void> {
    const account = await getAccountRequest(accountId);
    const accountIndex = state.budget.accounts.findIndex(x => x.id === account.id);
    state.budget.accounts[accountIndex] = account;
  }

  static async reloadFund(state: AppState, fundId: string): Promise<void> {
    const fund = await getFundRequest(fundId);
    const fundIndex = state.budget.funds.findIndex(x => x.id === fund.id);
    state.budget.funds[fundIndex] = fund;
  }

  static ensureDefined(actionName: string, ...payload: unknown[]): boolean {
    if (
      payload === null ||
      payload === undefined ||
      payload.some((x) => x === null || x === undefined)
    )
      throw new Error(`${actionName} action payload must be defined`);
    return true;
  }
}
