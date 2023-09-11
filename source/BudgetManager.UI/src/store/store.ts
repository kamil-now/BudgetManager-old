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
import { createFundRequest, deleteFundRequest, updateFundRequest } from '../api/fund-requests';
import { Allocation } from '@/models/allocation';
import { createAllocationRequest, deleteAllocationRequest, updateAllocationRequest } from '@/api/allocation-requests';
import { AccountTransfer } from '@/models/account-transfer';
import { FundTransfer } from '@/models/fund-transfer';
import { createAccountTransferRequest, updateAccountTransferRequest, deleteAccountTransferRequest } from '@/api/account-transfer-requests';
import { createFundTransferRequest, updateFundTransferRequest, deleteFundTransferRequest } from '@/api/fund-transfer-requests';
import { CurrencyExchange } from '@/models/currency-exchange';
import { createCurrencyExchangeRequest, updateCurrencyExchangeRequest, deleteCurrencyExchangeRequest } from '@/api/currency-exchange-requests';

export type AppState = {
  isLoading: boolean;
  isLoggedIn: boolean;
  isNewUser: boolean,
  budgetBalance: { balance: Balance, unallocated: Balance },
  accounts: Account[],
  funds: Fund[],
  incomes: Income[],
  expenses: Expense[],
  allocations: Allocation[],
  fundTransfers: FundTransfer[],
  accountTransfers: AccountTransfer[],
  currencyExchanges: CurrencyExchange[]
};
export type AppGetters = {
  // findIndexById: (state: AppState) => (id: string) => number;
};
export type AppActions = {
  createBudget(): void;
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

  createNewAllocation(accountTransfer: Allocation): void,
  updateAllocation(accountTransfer: Allocation): void;
  deleteAllocation(accountTransfer: Allocation): void;

  createNewFundTransfer(fundTransfer: FundTransfer): void,
  updateFundTransfer(fundTransfer: FundTransfer): void;
  deleteFundTransfer(fundTransfer: FundTransfer): void;
  
  createNewAccountTransfer(accountTransfer: AccountTransfer): void,
  updateAccountTransfer(accountTransfer: AccountTransfer): void;
  deleteAccountTransfer(accountTransfer: AccountTransfer): void;

  createNewCurrencyExchange(currencyExchange: CurrencyExchange): void,
  updateCurrencyExchange(currencyExchange: CurrencyExchange): void;
  deleteCurrencyExchange(currencyExchange: CurrencyExchange): void;
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
  allocations: [],
  fundTransfers: [],
  accountTransfers: [],
  currencyExchanges: [],
  budgetBalance: { balance: {} as Balance, unallocated: {} as Balance }
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
              this.budgetBalance = res.balance;
              this.accounts = res.accounts;
              this.funds = res.funds;
              this.incomes = res.incomes;
              this.expenses = res.expenses;
              this.allocations = res.allocations;
              this.fundTransfers = res.fundTransfers;
              this.accountTransfers = res.accountTransfers;
              this.currencyExchanges = res.currencyExchanges;
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
      await Utils.runAsyncOperation(this, () => axios.post<void>('api/budget'));
    },
    async updateUserSettings() {
      await axios.put('api/user-settings', { 
        accountsOrder: this.accounts.map(x => x.id),
        fundsOrder: this.funds.map(x => x.id)
      });
    },
    
    async createNewAccount(account: Account) {
      await Utils.runAsyncOperation(this, async (state) => {
        const id = await createAccountRequest(account);
        state.accounts.unshift({ ...account, id });
        this.fetchBudget(); // TODO fetch only affected funds/accounts 
        this.updateUserSettings();
      });
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
            this.fetchBudget(); // TODO fetch only affected funds/accounts 
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
          .then(id => {
            state.funds.unshift({ ...fund, id });
            this.updateUserSettings();
          })
      );
    },
    async updateFund(fund: Fund) {
      await Utils.runAsyncOperation(this, (state) => 
        updateFundRequest(fund)
          .then(() => {
            const fromState = state.funds.find(x => x.id === fund.id);
            if (!fromState) {
              throw new Error('Invalid operation - fund does not exist');
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
          .then(id => {
            state.expenses.unshift({ ...expense, id });
            this.fetchBudget(); // TODO fetch only affected funds/accounts 
          })
      );
    },
    async updateExpense(expense: Expense) {
      await Utils.runAsyncOperation(this, (state) => 
        updateExpenseRequest(expense)
          .then(expense => {
            const fromState = state.expenses.find(x => x.id === expense.id);
            if (!fromState) {
              throw new Error('Invalid operation - expense does not exist');
            }
            const index = state.expenses.indexOf(fromState);
            state.expenses[index] = expense; 
            this.fetchBudget(); // TODO fetch only affected funds/accounts 
          })
      );
    },
    async deleteExpense(expense: Expense) {
      await Utils.runAsyncOperation(this, () => 
        deleteExpenseRequest(expense)
          .then(() => this.fetchBudget())// TODO fetch only affected funds/accounts 
      );
    },

    async createNewIncome(
      income: Income,
    ) {
      await Utils.runAsyncOperation(this, (state) => 
        createIncomeRequest(income)
          .then(id => {
            state.incomes.unshift({ ...income, id }); 
            this.fetchBudget(); // TODO fetch only affected funds/accounts 
          })
      );
    },
    async updateIncome(income: Income) {
      await Utils.runAsyncOperation(this, (state) => 
        updateIncomeRequest(income)
          .then(income => {
            const fromState = state.incomes.find(x => x.id === income.id);
            if (!fromState) {
              throw new Error('Invalid operation - income does not exist');
            }
            const index = state.incomes.indexOf(fromState);
            state.incomes[index] = income; 
            this.fetchBudget(); // TODO fetch only affected funds/accounts 
          })
      );
    },
    async deleteIncome(income: Income) {
      await Utils.runAsyncOperation(this, () => 
        deleteIncomeRequest(income)
          .then(() => this.fetchBudget())// TODO fetch only affected funds/accounts 
      );
    },
    async createNewAllocation(
      allocation: Allocation,
    ) {
      await Utils.runAsyncOperation(this, (state) => 
        createAllocationRequest(allocation)
          .then(id => {
            state.allocations.unshift({ ...allocation, id }); 
            this.fetchBudget(); // TODO fetch only affected funds/accounts 
          })
      );
    },
    async updateAllocation(allocation: Allocation) {
      await Utils.runAsyncOperation(this, (state) => 
        updateAllocationRequest(allocation)
          .then(allocation => {
            const fromState = state.allocations.find(x => x.id === allocation.id);
            if (!fromState) {
              throw new Error('Invalid operation - allocation does not exist');
            }
            const index = state.allocations.indexOf(fromState);
            state.allocations[index] = allocation; 
            this.fetchBudget(); // TODO fetch only affected funds/accounts 
          })
      );
    },
    async deleteAllocation(allocation: Allocation) {
      await Utils.runAsyncOperation(this, () => 
        deleteAllocationRequest(allocation)
          .then(() => this.fetchBudget())// TODO fetch only affected funds/accounts 
      );
    },
    async createNewFundTransfer(
      fundTransfer: FundTransfer,
    ) {
      await Utils.runAsyncOperation(this, (state) => 
        createFundTransferRequest(fundTransfer)
          .then(id => {
            state.fundTransfers.unshift({ ...fundTransfer, id }); 
            this.fetchBudget(); // TODO fetch only affected funds/accounts 
          })
      );
    },
    async updateFundTransfer(fundTransfer: FundTransfer) {
      await Utils.runAsyncOperation(this, (state) => 
        updateFundTransferRequest(fundTransfer)
          .then(fundTransfer => {
            const fromState = state.fundTransfers.find(x => x.id === fundTransfer.id);
            if (!fromState) {
              throw new Error('Invalid operation - fund transfer does not exist');
            }
            const index = state.fundTransfers.indexOf(fromState);
            state.fundTransfers[index] = fundTransfer; 
            this.fetchBudget(); // TODO fetch only affected funds/accounts 
          })
      );
    },
    async deleteFundTransfer(fundTransfer: FundTransfer) {
      await Utils.runAsyncOperation(this, () => 
        deleteFundTransferRequest(fundTransfer)
          .then(() => this.fetchBudget())// TODO fetch only affected funds/accounts 
      );
    },
    async createNewAccountTransfer(
      accountTransfer: AccountTransfer,
    ) {
      await Utils.runAsyncOperation(this, (state) => 
        createAccountTransferRequest(accountTransfer)
          .then(id => {
            state.accountTransfers.unshift({ ...accountTransfer, id }); 
            this.fetchBudget(); // TODO fetch only affected funds/accounts 
          })
      );
    },
    async updateAccountTransfer(accountTransfer: AccountTransfer) {
      await Utils.runAsyncOperation(this, (state) => 
        updateAccountTransferRequest(accountTransfer)
          .then(accountTransfer => {
            const fromState = state.accountTransfers.find(x => x.id === accountTransfer.id);
            if (!fromState) {
              throw new Error('Invalid operation - account transfer does not exist');
            }
            const index = state.accountTransfers.indexOf(fromState);
            state.accountTransfers[index] = accountTransfer; 
            this.fetchBudget(); // TODO fetch only affected funds/accounts 
          })
      );
    },
    async deleteAccountTransfer(accountTransfer: AccountTransfer) {
      await Utils.runAsyncOperation(this, () => 
        deleteAccountTransferRequest(accountTransfer)
          .then(() => this.fetchBudget())// TODO fetch only affected funds/accounts 
      );
    },
    async createNewCurrencyExchange(
      currencyExchange: CurrencyExchange,
    ) {
      await Utils.runAsyncOperation(this, (state) => 
        createCurrencyExchangeRequest(currencyExchange)
          .then(id => {
            state.currencyExchanges.unshift({ ...currencyExchange, id }); 
            this.fetchBudget(); // TODO fetch only affected funds/accounts 
          })
      );
    },
    async updateCurrencyExchange(currencyExchange: CurrencyExchange) {
      await Utils.runAsyncOperation(this, (state) => 
        updateCurrencyExchangeRequest(currencyExchange)
          .then(currencyExchange => {
            const fromState = state.currencyExchanges.find(x => x.id === currencyExchange.id);
            if (!fromState) {
              throw new Error('Invalid operation - account transfer does not exist');
            }
            const index = state.currencyExchanges.indexOf(fromState);
            state.currencyExchanges[index] = currencyExchange; 
            this.fetchBudget(); // TODO fetch only affected funds/accounts 
          })
      );
    },
    async deleteCurrencyExchange(currencyExchange: CurrencyExchange) {
      await Utils.runAsyncOperation(this, () => 
        deleteCurrencyExchangeRequest(currencyExchange)
          .then(() => this.fetchBudget())// TODO fetch only affected funds/accounts 
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
