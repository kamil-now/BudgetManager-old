import { createFundRequest, deleteFundRequest, getFundRequest, updateFundRequest } from '@/api/fund-requests';
import { Fund } from '@/models/fund';
import { AppStore } from '../store';
import { StoreUtils } from '../store-utils';

export interface IFundActions { 
  createNewFund(fund: Fund): void,
  updateFund(fund: Fund): void;
  deleteFund(fundId: string): void;
}

export class FundActions {
  static async createNewFund(store: AppStore, fund: Fund) {
    await StoreUtils.runAsyncOperation(store, async (state) => {
      const id = await createFundRequest(fund);
      const fromResponse = await getFundRequest(id);
      state.budget.funds.unshift(fromResponse);
      store.updateUserSettings();
    });
  }

  static async updateFund(store: AppStore, fund: Fund) {
    await StoreUtils.runAsyncOperation(store, async () => {
      const fromResponse = await updateFundRequest(fund);
      StoreUtils.replaceInCollection(store.budget.funds, fromResponse);
    });
  }

  static async deleteFund(store: AppStore, fundId: string) {
    await StoreUtils.runAsyncOperation(store, async () => {
      await deleteFundRequest(fundId);
      store.budget.funds = store.budget.funds.filter(x => x.id !== fundId);
    });
  }
}
