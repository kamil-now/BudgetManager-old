import { Account } from '@/models/account';
import { Budget } from '@/models/budget';
import { Fund } from '@/models/fund';
import axios from 'axios';
import { defineStore, DefineStoreOptions, Store } from 'pinia';
import { createBudget } from './create-budget';

export type AppState = {
  isLoading: boolean;
  isLoggedIn: boolean;
  undoStack: ((state: AppState) => void)[];
  budget?: Budget
};
export type AppGetters = {
  // findIndexById: (state: AppState) => (id: string) => number;
};
export type AppActions = {
  createBudget(defaultFundName: string, defaultCurrency: string, accounts: Account[], funds: Fund[]): void;setLoggedIn(value: boolean): void;
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
  actions: {
    setLoggedIn(value: boolean) {
      this.isLoggedIn = value;
    },
    async createBudget(
      defaultFundName: string,
      defaultCurrency: string,
      accounts: Account[],
      funds: Fund[]
    ) {
      const budget = { defaultFundName, defaultCurrency, accounts, funds };
      await Utils.runAsyncOperation(this, () =>
        createBudget(defaultFundName, accounts, funds)
          .then(() => this.budget = budget)
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
  },
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
