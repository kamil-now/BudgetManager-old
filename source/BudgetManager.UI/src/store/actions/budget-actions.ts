import { fetchBudgetSummary } from '@/api/fetch-budget-request';
import axios from 'axios';
import { AppStore } from '../store';
import { StoreUtils } from '../store-utils';

export interface IBudgetActions { 
  createBudget(): void;
  fetchBudget(): void;
}

export class BudgetActions {
  static async createBudget(store: AppStore) {
    await StoreUtils.runAsyncOperation(store, () => axios.post<void>('budget'));
  }
  
  static async fetchBudget(store: AppStore) {
    await StoreUtils.runAsyncOperation(store, () =>
      fetchBudgetSummary()
        .then(res => {
          if (res !== null) {
            store.budget = res;
            store.isNewUser = false;
          } else {
            store.isNewUser = true;
          }
        },
        (error) => {
          console.error(error);
        })
    );
  }
}
