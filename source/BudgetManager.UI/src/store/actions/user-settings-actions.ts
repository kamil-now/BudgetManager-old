import axios from 'axios';
import { AppStore } from '../store';

export interface IUserSettingsActions { 
  updateUserSettings(): void;
  reorderFunds(oldIndex: number, newIndex: number): void;
  reorderAccounts(oldIndex: number, newIndex: number): void;
}

export class UserSettingsActions {
  static async reorderAccounts(store: AppStore, oldIndex: number, newIndex: number) {
    oldIndex = store.budget.accounts.indexOf(store.accounts[oldIndex]);
    newIndex = store.budget.accounts.indexOf(store.accounts[newIndex]);
    const account = store.budget.accounts.splice(oldIndex, 1)[0];
    store.budget.accounts.splice(newIndex, 0, account);
    store.updateUserSettings();
  }

  static async reorderFunds(store: AppStore, oldIndex: number, newIndex: number) {
    oldIndex = store.budget.funds.indexOf(store.funds[oldIndex]);
    newIndex = store.budget.funds.indexOf(store.funds[newIndex]);
    const fund = store.budget.funds.splice(oldIndex, 1)[0];
    store.budget.funds.splice(newIndex, 0, fund);
    store.updateUserSettings();
  }

  static async updateUserSettings(store: AppStore) {
    await axios.put('user-settings', { 
      accountsOrder: store.budget.accounts.filter(x => !x.isDeleted).map(x => x.id),
      fundsOrder: store.budget.funds.filter(x => !x.isDeleted).map(x => x.id)
    });
  }
}
