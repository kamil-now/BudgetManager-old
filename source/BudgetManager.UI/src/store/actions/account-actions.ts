import { createAccountRequest, deleteAccountRequest, getAccountRequest, updateAccountRequest } from '@/api/account-requests';
import { Account } from '@/models/account';
import { AppStore } from '../store';
import { StoreUtils } from '../store-utils';

export interface IAccountActions { 
  createNewAccount(account: Account): void,
  updateAccount(account: Account): void;
  deleteAccount(accountId: string): void;
}

export class AccountActions {
  static async createNewAccount(store: AppStore, account: Account) {
    await StoreUtils.runAsyncOperation(store, async () => {
      const id = await createAccountRequest(account);
      const fromResponse = await getAccountRequest(id);
      store.budget.accounts.unshift(fromResponse);
      await StoreUtils.reloadBalance(store);
      store.updateUserSettings();
    });
  }
  
  static async updateAccount(store: AppStore, account: Account) {
    await StoreUtils.runAsyncOperation(store, async () => {
      const fromResponse = await updateAccountRequest(account);
      StoreUtils.replaceInCollection(store.budget.accounts, fromResponse);
      await StoreUtils.reloadBalance(store);
    });
  }

  static async deleteAccount(store: AppStore, accountId: string) {
    await StoreUtils.runAsyncOperation(store, async () => {
      await deleteAccountRequest(accountId);
      store.budget.accounts = store.budget.accounts.filter(x => x.id !== accountId);
      await StoreUtils.reloadBalance(store);
    });
  }
}
