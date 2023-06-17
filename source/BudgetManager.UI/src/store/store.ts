import { Account } from '@/models/account';
import { Budget } from '@/models/budget';
import { Fund } from '@/models/fund';
import axios from 'axios';
import { defineStore, DefineStoreOptions, Store } from 'pinia';
import { createBudgetRequest } from './create-budget-request';
import { fetchBudgetRequest } from './fetch-budget-request';

export type AppState = {
  isLoading: boolean;
  isLoggedIn: boolean;
  undoStack: ((state: AppState) => void)[];
  budget?: Budget
};
export type AppGetters = {
  isNewUser: (state: AppState) => boolean
  // findIndexById: (state: AppState) => (id: string) => number;
};
export type AppActions = {
  createBudget(accounts: Account[], funds: Fund[]): void; 
  setLoggedIn(value: boolean): void;
  fetchBudget(): void;
  save(): void;
  undo(): void;
};
export type AppStore = Store<string, AppState, AppGetters, AppActions>;

export const getInitialAppState: () => AppState = () => ({
  isLoading: true,
  isLoggedIn: false,
  undoStack: [],
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
    isNewUser: (state: AppState) => !state.budget
  },
  actions: {
    async setLoggedIn(value: boolean) {
      this.isLoggedIn = value;
      this.fetchBudget();
    },
    async fetchBudget() {
      await Utils.runAsyncOperation(this, () =>
        fetchBudgetRequest()
          .then(budget => {
            if (budget !== null) {
              this.budget = budget;
            }
            this.isLoading = false;
          })
      );
    },
    async createBudget(
      accounts: Account[],
      funds: Fund[]
    ) {
      const budget = {  accounts, funds };
      await Utils.runAsyncOperation(this, () =>
        createBudgetRequest(accounts, funds)
          .then(balance => this.budget = { ...budget, balance })
      );
    },
    async save() {
      await Utils.runAsyncOperation(this, (state) =>
        axios.patch('api/', { state })
      );
    },

    undo() {
      const action = this.undoStack.pop();
      if (action) {
        action(this);
        this.save();
      } else {
        console.error('Invalid undo operation');
      }
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
