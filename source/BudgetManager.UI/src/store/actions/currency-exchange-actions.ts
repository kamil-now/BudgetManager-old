import { createCurrencyExchangeRequest, deleteCurrencyExchangeRequest, getCurrencyExchangeRequest, updateCurrencyExchangeRequest } from '@/api/currency-exchange-requests';
import { CurrencyExchange } from '@/models/currency-exchange';
import { AppStore } from '../store';
import { StoreUtils } from '../store-utils';

export interface ICurrencyExchangeActions {
  createNewCurrencyExchange(currencyExchange: CurrencyExchange): void,
  updateCurrencyExchange(currencyExchange: CurrencyExchange): void;
  deleteCurrencyExchange(currencyExchangeId: string): void; 
}

export class CurrencyExchangeActions {
  static async createNewCurrencyExchange(store: AppStore, currencyExchange: CurrencyExchange) {
    await StoreUtils.runAsyncOperation(store, async (state) => {
      const fromResponse = await StoreUtils.createOperation(
        state,
        () => createCurrencyExchangeRequest(currencyExchange),
        id => getCurrencyExchangeRequest(id)
      );
      await this.reload(store, fromResponse);
    });
  }

  static async updateCurrencyExchange(store: AppStore, currencyExchange: CurrencyExchange) {
    await StoreUtils.runAsyncOperation(store, async (state) => {
      const fromResponse = await StoreUtils.updateOperation(state, () => updateCurrencyExchangeRequest(currencyExchange));
      await this.reload(store, fromResponse);
    });
  }

  static async deleteCurrencyExchange(store: AppStore, currencyExchangeId: string) {
    await StoreUtils.runAsyncOperation(store, async () => {
      await deleteCurrencyExchangeRequest(currencyExchangeId);
      await this.reload(store, StoreUtils.getFromCollection(store.currencyExchanges, currencyExchangeId));
    });
  }

  private static async reload(store: AppStore, currencyExchange: CurrencyExchange) {
    await StoreUtils.reloadAccount(store, currencyExchange.accountId);
    await StoreUtils.reloadBalance(store);
  }
}
