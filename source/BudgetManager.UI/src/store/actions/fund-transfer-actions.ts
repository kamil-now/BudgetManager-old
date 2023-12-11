import { createFundTransferRequest, deleteFundTransferRequest, getFundTransferRequest, updateFundTransferRequest } from '@/api/fund-transfer-requests';
import { FundTransfer } from '@/models/fund-transfer';
import { AppStore } from '../store';
import { StoreUtils } from '../store-utils';

export interface IFundTransferActions { 
  createNewFundTransfer(fundTransfer: FundTransfer): void,
  updateFundTransfer(fundTransfer: FundTransfer): void;
  deleteFundTransfer(fundTransferId: string): void;
}

export class FundTransferActions {
  static async createNewFundTransfer(store: AppStore, fundTransfer: FundTransfer) {
    await StoreUtils.runAsyncOperation(store, async () => {
      const fromResponse = await StoreUtils.createOperation(
        store,
        () => createFundTransferRequest(fundTransfer),
        id => getFundTransferRequest(id)
      );
      await this.reload(store, fromResponse);
    });
  }

  static async updateFundTransfer(store: AppStore, fundTransfer: FundTransfer) {
    await StoreUtils.runAsyncOperation(store, async () => {
      const fromResponse = await StoreUtils.updateOperation(store, () => updateFundTransferRequest(fundTransfer));
      await this.reload(store, fromResponse);
    });
  }

  static async deleteFundTransfer(store: AppStore, fundTransferId: string) {
    await StoreUtils.runAsyncOperation(store, async () => {
      await deleteFundTransferRequest(fundTransferId);
      await this.reload(store, StoreUtils.getFromCollection(store.fundTransfers, fundTransferId));
      store.budget.operations = store.budget.operations.filter(x => x.id !== fundTransferId);
    });
  }

  private static async reload(store: AppStore, fundTransfer: FundTransfer) {
    await StoreUtils.reloadFund(store, fundTransfer.fundId);
    await StoreUtils.reloadFund(store, fundTransfer.targetFundId);
    await StoreUtils.reloadBalance(store);
  }
}
