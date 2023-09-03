import { createExpenseRequest, deleteExpenseRequest, updateExpenseRequest } from '@/api/expense-requests';
import { createIncomeRequest, deleteIncomeRequest, updateIncomeRequest } from '@/api/income-requests';
import { Account } from '@/models/account';
import { Balance } from '@/models/balance';
import { Expense } from '@/models/expense';
import { Fund } from '@/models/fund';
import { Income } from '@/models/income';
import axios from 'axios';
import { defineStore, DefineStoreOptions, Store } from 'pinia';
import { createAccountRequest, deleteAccountRequest, updateAccountRequest } from '../api/account-requests';
import { fetchBudgetRequest } from '../api/fetch-budget-request';
import { createFundRequest, deleteFundRequest, getFundRequest, updateFundRequest } from '../api/fund-requests';

export type AppState = {
  isLoading: boolean;
  isLoggedIn: boolean;
  isNewUser: boolean,
  balance?: Balance,
  accounts: Account[],
  funds: Fund[],
  incomes: Income[],
  expenses: Expense[],
};
export type AppGetters = {
  // findIndexById: (state: AppState) => (id: string) => number;
};
export type AppActions = {
  createBudget(accounts: Account[], funds: Fund[]): void;
  updateUserSettings(): void,
  setLoggedIn(value: boolean): void;
  fetchBudget(): void;

  createNewAccount(account: Account): void,
  updateAccount(account: Account): void;
  deleteAccount(account:Account): void;

  createNewFund(fund: Fund): void,
  updateFund(fund: Fund): void;
  deleteFund(fund: Fund): void;
  
  createNewIncome(income: Income): void,
  updateIncome(income: Income): void;
  deleteIncome(income: Income): void;

  createNewExpense(expense: Expense): void,
  updateExpense(expense: Expense): void;
  deleteExpense(expense: Expense): void;
};
export type AppStore = Store<string, AppState, AppGetters, AppActions>;

export const getInitialAppState: () => AppState = () => ({
  isLoading: true,
  isLoggedIn: false,
  isNewUser: true,
  accounts: [],
  funds: [],
  incomes: [],
  expenses: [],
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

  },
  actions: {
    async setLoggedIn(value: boolean) {
      this.isLoggedIn = value;
      this.fetchBudget();
    },
    async fetchBudget() {
      await Utils.runAsyncOperation(this, () =>
        fetchBudgetRequest()
          .then(res => {
            if (res !== null) {
              this.balance = res.balance;
              this.accounts = res.accounts;
              this.funds = res.funds;
              this.incomes = res.incomes;
              this.expenses = res.expenses;
              this.isNewUser = false;
            } else {
              this.isNewUser = true;
            }
          })
      );
    },
    async createBudget(
      accounts: Account[],
      funds: Fund[]
    ) {
      await Utils.runAsyncOperation(this, () => 
        axios.post<void>('api/budget')
          .then(() =>
            funds.length > 0
              ? funds.map(fund => createFundRequest(fund))
              : [Promise.resolve()])
          .then(() =>
            accounts.length > 0
              ? accounts
                .map(account => createAccountRequest(account))
              : [Promise.resolve()])
          .then(() => this.fetchBudget())
          .then(() => this.updateUserSettings())
      );
    },
    async updateUserSettings() {
      await axios.put('api/user-settings', { 
        accountsOrder: this.accounts.map(x => x.id),
        fundsOrder: this.funds.map(x => x.id)
      });
    },
    
    async createNewAccount(account: Account) {
      await Utils.runAsyncOperation(this, (state) => 
        createAccountRequest(account)
          .then(async id => {
            const fromState = state.accounts.find(x => x.id === id);
            if (!fromState) {
              state.accounts.unshift({ ...account, id });
            } else {
              const index = state.accounts.indexOf(fromState);
              state.accounts[index] = account; 
            }

            const defaultFund = state.funds.find(x => x.isDefault);
            if (defaultFund) {
              state.funds[state.funds.indexOf(defaultFund)] = await getFundRequest(defaultFund);
            }
          })
      );
    },
    async updateAccount(account: Account) {
      await Utils.runAsyncOperation(this, (state) => 
        updateAccountRequest(account)
          .then(account => {
            const fromState = state.accounts.find(x => x.id === account.id);
            if (!fromState) {
              throw new Error('Invalid operation - account does not exist');
            }
            const index = state.accounts.indexOf(fromState);
            state.accounts[index] = account; 
          })
      );
    },
    async deleteAccount(account:Account) {
      await Utils.runAsyncOperation(this, (state) => 
        deleteAccountRequest(account)
          .then(() => state.accounts.splice(state.accounts.indexOf(account), 1))
      );
    },

    async createNewFund(
      fund: Fund,
    ) {
      await Utils.runAsyncOperation(this, (state) => 
        createFundRequest(fund)
          .then(id => state.funds.unshift({ ...fund, id }))
      );
    },
    async updateFund(fund: Fund) {
      await Utils.runAsyncOperation(this, (state) => 
        updateFundRequest(fund)
          .then(fund => {
            const fromState = state.funds.find(x => x.id === fund.id);
            if (!fromState) {
              throw new Error('Invalid operation - account does not exist');
            }
            const index = state.funds.indexOf(fromState);
            state.funds[index] = fund; 
          })
      );
    },
    async deleteFund(fund: Fund) {
      await Utils.runAsyncOperation(this, (state) => 
        deleteFundRequest(fund)
          .then(() => state.funds.splice(state.funds.indexOf(fund), 1))
      );
    },

    async createNewExpense(
      expense: Expense,
    ) {
      await Utils.runAsyncOperation(this, (state) => 
        createExpenseRequest(expense)
          .then(id => state.expenses.unshift({ ...expense, id }))
      );
    },
    async updateExpense(expense: Expense) {
      await Utils.runAsyncOperation(this, (state) => 
        updateExpenseRequest(expense)
          .then(expense => {
            const fromState = state.expenses.find(x => x.id === expense.id);
            if (!fromState) {
              throw new Error('Invalid operation - account does not exist');
            }
            const index = state.expenses.indexOf(fromState);
            state.expenses[index] = expense; 
          })
      );
    },
    async deleteExpense(expense: Expense) {
      await Utils.runAsyncOperation(this, (state) => 
        deleteExpenseRequest(expense)
          .then(() => state.expenses.splice(state.expenses.indexOf(expense), 1))
      );
    },

    async createNewIncome(
      income: Income,
    ) {
      await Utils.runAsyncOperation(this, (state) => 
        createIncomeRequest(income)
          .then(id => state.incomes.unshift({ ...income, id }))
      );
    },
    async updateIncome(income: Income) {
      await Utils.runAsyncOperation(this, (state) => 
        updateIncomeRequest(income)
          .then(income => {
            const fromState = state.incomes.find(x => x.id === income.id);
            if (!fromState) {
              throw new Error('Invalid operation - account does not exist');
            }
            const index = state.incomes.indexOf(fromState);
            state.incomes[index] = income; 
          })
      );
    },
    async deleteIncome(income: Income) {
      await Utils.runAsyncOperation(this, (state) => 
        deleteIncomeRequest(income)
          .then(() => state.incomes.splice(state.incomes.indexOf(income), 1))
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
